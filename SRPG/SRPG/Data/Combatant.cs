﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Torch;
using Game = Microsoft.Xna.Framework.Game;

namespace SRPG.Data
{
    public class Combatant : GameComponent
    {
        public string Name;
        /// <summary>
        /// Character's current maximum health pool.
        /// </summary>
        public int MaxHealth;
        /// <summary>
        /// Character's current maximum mana pool.
        /// </summary>
        public int MaxMana;
        /// <summary>
        /// Character's currently available health pool. The character will die if this reaches 0.
        /// </summary>
        public int CurrentHealth;
        /// <summary>
        /// Character's currently available mana pool. Each time an active ability is used, this is reduced.
        /// </summary>
        public int CurrentMana;
        /// <summary>
        /// DAWISH stat list for this character. This list is the unmodified list, and thus ignores status ailments,
        /// buffs and debuffs
        /// </summary>
        public Dictionary<Stat, int> Stats = new Dictionary<Stat, int>()
            {
                {Stat.Defense, 0},
                {Stat.Attack, 0},
                {Stat.Wisdom, 0},
                {Stat.Intelligence, 0},
                {Stat.Speed, 0},
                {Stat.Hit, 0}
            };
        /// <summary>
        /// Temporary modifications to a character's DAWISH stats, such as from passive abilities, buffs, debuffs
        /// or status ailments such as blind.
        /// </summary>
        public Dictionary<Stat, int> BonusStats;

        /// <summary>
        /// List of items currently equipped by the character, including weapon, armor and accessory.
        /// </summary>
        public List<Item> Inventory = new List<Item>() { };
        /// <summary>
        /// A small portrait to be shown when this character is highlighted.
        /// </summary>
        public SpriteObject Portrait;
        /// <summary>
        /// Human readable class name.
        /// </summary>
        public string Class;
        /// <summary>
        /// Flags indicating what armor types this class can equip.
        /// </summary>
        public ItemType ArmorTypes;
        /// <summary>
        /// Flags indicating what weapon types this class can equip.
        /// </summary>
        public ItemType WeaponTypes;
        /// <summary>
        /// An integer indicating what faction this character belongs to - 0 represents the player and 1 represents the enemy.
        /// This is used to determine targeting, AI control, player control, and character movement grids.
        /// </summary>
        public int Faction;
        /// <summary>
        /// A boolean value indicating whether or not the character is able to move in the current turn. This is set to true at the start
        /// of a round, and false upon moving. Status ailments such as sleep and stun can set this to false during BeginRound.
        /// </summary>
        public bool CanMove;
        /// <summary>
        /// A boolean value indicating whether or not the character is able to act in the current turn. An action is defined as using an
        /// item, attacking, using an active ability, or switching weapons. This is set to true at the start of the round, but can
        /// be set to false due to sleep or stun.
        /// </summary>
        public bool CanAct;

        public Avatar Avatar;

        public Dictionary<Stat, int> StatExperienceLevels;
        public Dictionary<Ability, int> AbilityExperienceLevels = new Dictionary<Ability, int>();

        public Combatant(Game game) : base(game) { }

        /// <summary>
        /// Process any status ailments that would impact a character at the start of the round,
        /// such as sleep or stun.
        /// </summary>
        public void BeginRound()
        {
            CanMove = true;
            CanAct = true;
        }

        /// <summary>
        /// Process any status ailments that would impact a character at the end of the round, such as poison.
        /// Also check to see if any status ailments naturally were removed, or if any passive abilities can
        /// be processed, such as focus.
        /// </summary>
        public void EndRound()
        {
            CanMove = false;
            CanAct = false;
        }

        /// <summary>
        /// Receive a hit and modify it to represent actual damage inflicted. This will take into account
        /// defense and other passive abilities. The hit returned can then be passed to the ReceiveHit
        /// method to execute the hit, or simply read for AI processing.
        /// </summary>
        /// <param name="hit">A hit generated by a call to Ability.GenerateHits().</param>
        public Hit ProcessHit(Hit hit)
        {
            return hit;
        }

        /// <summary>
        /// Inflict the damage/healing/status ailments from a hit. The hit should be run through ProcessHit
        /// beforehand to ensure that damage is adjusted by armor and passive abilities.
        /// </summary>
        /// <param name="hit"></param>
        public void ReceiveHit(Hit hit)
        {
            if (hit.Damage > 0)
            {
                CurrentHealth -= hit.Damage;
                return;
            }

            var heal = hit.Damage < CurrentHealth - MaxHealth ? CurrentHealth - MaxHealth : hit.Damage;
            CurrentHealth += Math.Abs(heal);
            return;
        }

        /// <summary>
        /// Indicate whether or not the character is being threatened. Being threatened is defined as being within attack
        /// range of any regular attacks. Regular attacks do not include active or passive abilities and are strictly limited
        /// to weapon attacks.
        /// </summary>
        /// <param name="board">The game board for the current battle. This will be asked about enemy positioning.</param>
        /// <returns></returns>
        public bool IsThreatened(BattleBoard board)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Raise the character's experience levels, in accordance with the stat multipliers granted.
        /// </summary>
        /// <param name="experience">amount of experience points gained</param>
        public void GainExperience(int experience)
        {
            throw new NotImplementedException();
        }

        public void Die()
        {

        }

        public int ReadStat(Stat stat)
        {
            var number = Stats[stat];

            if (GetEquippedWeapon() != null) number += GetEquippedWeapon().StatBoosts[stat];
            if (GetEquippedArmor() != null) number += GetEquippedArmor().StatBoosts[stat];
            if (GetEquippedAccessory() != null) number += GetEquippedAccessory().StatBoosts[stat];

            return number;
        }

        public Dictionary<Stat, int> ReadAllStats()
        {
            return new Dictionary<Stat, int>
                {
                    {Stat.Defense, ReadStat(Stat.Defense)},
                    {Stat.Attack, ReadStat(Stat.Attack)},
                    {Stat.Wisdom, ReadStat(Stat.Wisdom)},
                    {Stat.Intelligence, ReadStat(Stat.Intelligence)},
                    {Stat.Speed, ReadStat(Stat.Speed)},
                    {Stat.Hit, ReadStat(Stat.Hit)}
                };
        }

        /// <summary>
        /// Return a list of abilities that this character is capable of performing. This includes learned abilities,
        /// abilities derived from the currently equipped weapon/armor, and disabled abilities that apply to other
        /// weapon/armor types.
        /// </summary>
        /// <returns></returns>
        public List<Ability> GetAbilities()
        {
            // todo: so much boilerplate with checking equipped gear and avoiding null...

            // get known abilities
            var abilities = (from ability in AbilityExperienceLevels where ability.Value > 100 select ability.Key).ToList();

            var weapon = GetEquippedWeapon();
            if (weapon != null && !abilities.Contains(weapon.Ability) && weapon.Ability != null) abilities.Add(weapon.Ability);

            var armor = GetEquippedArmor();
            if (armor != null && !abilities.Contains(armor.Ability) && armor.Ability != null) abilities.Add(armor.Ability);

            foreach(var ability in abilities)
            {
                ability.Character = this;
            }

            // todo remove duplicate abilities
            return abilities;
        }

        /// <summary>
        /// Return true or false to indicate if this character can use the specified ability. This is dependent
        /// on the weapon and armor currently equipped and should only be used in conjuction with GetAbilities.
        /// </summary>
        /// <param name="ability"></param>
        /// <returns></returns>
        public bool CanUseAbility(Ability ability)
        {
            var weapon = GetEquippedWeapon();
            if (weapon != null && ability.ItemType == weapon.ItemType) return true;

            var armor = GetEquippedArmor();
            if (armor != null && ability.ItemType == armor.ItemType) return true;

            return false;
        }

        public int GenerateExperience()
        {
            throw new NotImplementedException();
        }

        public Item GetEquippedWeapon()
        {
            return (from item in Inventory where item.ItemType == WeaponTypes select item).FirstOrDefault();
        }

        public Item GetEquippedArmor()
        {
            return (from item in Inventory where item.ItemType == ArmorTypes select item).FirstOrDefault();
        }

        public Item GetEquippedAccessory()
        {
            return (from item in Inventory where item.ItemType == ItemType.Accessory select item).FirstOrDefault();
        }

        public Item EquipItem(Item item)
        {
            var oldItem = new Item(Game);

            switch (item.GetEquipType())
            {
                case ItemEquipType.Weapon:
                    oldItem = GetEquippedWeapon();
                    break;
                case ItemEquipType.Armor:
                    oldItem = GetEquippedArmor();
                    break;
                case ItemEquipType.Accessory:
                    throw new NotImplementedException();
                    break;

                default:
                    throw new Exception("unable to equip this item");
            }

            if (oldItem != null) Inventory.Remove(oldItem);
            Inventory.Add(item);

            return oldItem;
        }

        /// <summary>
        /// Indicate if this class is able to equip the specified armor.
        /// </summary>
        /// <param name="armor">The armor that is being requested.</param>
        /// <returns>true if this class can equip this armor.</returns>
        public bool CanEquipArmor(Item armor)
        {
            return (ArmorTypes & armor.ItemType) == armor.ItemType;
        }

        /// <summary>
        /// Indicate if this class is able to equip the specified weapon.
        /// </summary>
        /// <param name="weapon">The weapon that is being requested.</param>
        /// <returns>true if this class can equip this weapon.</returns>
        public bool CanEquipWeapon(Item weapon)
        {
            return (WeaponTypes & weapon.ItemType) == weapon.ItemType;
        }

        /// <summary>
        /// Generate a combatant based on a predefined template
        /// </summary>
        /// <param name="template">Template to be used to generate a combatant</param>
        /// <param name="level">Experience level to be applied to this combatant's skills</param>
        /// <returns>A combatant generated from the template, modified to be at the specified level.</returns>
        public static Combatant FromTemplate(Microsoft.Xna.Framework.Game game, string template)
        {
            var combatant = new Combatant(game);

            var templateType = template.Split('/')[0];
            var templateName = template.Split('/')[1];

            string settingString = String.Join("\r\n", File.ReadAllLines("Content/Battle/Combatants/" + templateType + ".js"));

            var nodeList = JObject.Parse(settingString);

            combatant.Class = nodeList[templateName]["class"].ToString();
            combatant.Avatar = Avatar.GenerateAvatar(game, null, nodeList[templateName]["avatar"].ToString());

            combatant.CurrentHealth = (int)(nodeList[templateName]["health"]);
            combatant.MaxHealth = (int)(nodeList[templateName]["health"]);
            combatant.CurrentMana = (int)(nodeList[templateName]["mana"]);
            combatant.MaxMana = (int)(nodeList[templateName]["mana"]);

            combatant.ArmorTypes = Item.StringToItemType(nodeList[templateName]["armortype"].ToString());
            combatant.WeaponTypes = Item.StringToItemType(nodeList[templateName]["weapontype"].ToString());

            combatant.Inventory = nodeList[templateName]["inventory"].Select(item => Item.Factory(game, item.ToString())).ToList();

            combatant.Stats = new Dictionary<Stat, int>
                {
                    {Stat.Defense, (int)(nodeList[templateName]["stats"]["defense"])},
                    {Stat.Attack, (int)(nodeList[templateName]["stats"]["attack"])},
                    {Stat.Wisdom, (int)(nodeList[templateName]["stats"]["wisdom"])},
                    {Stat.Intelligence, (int)(nodeList[templateName]["stats"]["intelligence"])},
                    {Stat.Speed, (int)(nodeList[templateName]["stats"]["speed"])},
                    {Stat.Hit, (int)(nodeList[templateName]["stats"]["hit"])},
                };

            if (nodeList[templateName].SelectToken("statxp") != null)
            {
                combatant.StatExperienceLevels = new Dictionary<Stat, int>
                    {
                        {Stat.Defense, (int) (nodeList[templateName]["statxp"]["defense"])},
                        {Stat.Attack, (int) (nodeList[templateName]["statxp"]["attack"])},
                        {Stat.Wisdom, (int) (nodeList[templateName]["statxp"]["wisdom"])},
                        {Stat.Intelligence, (int) (nodeList[templateName]["statxp"]["intelligence"])},
                        {Stat.Speed, (int) (nodeList[templateName]["statxp"]["speed"])},
                        {Stat.Hit, (int) (nodeList[templateName]["statxp"]["hit"])}
                    };
            }

            combatant.AbilityExperienceLevels = nodeList[templateName]["abilities"].ToDictionary(
                ability => Ability.Factory(game, ability["name"].ToString()), 
                ability => (int) ability["experience"]
            );

            return combatant;
        }

        public Grid GetMovementGrid(Grid battleboard)
        {
            var grid = new Grid(battleboard.Size.Width, battleboard.Size.Height);

            var neighbors = new List<int[]> { new[] { 0, -1 }, new[] { 1, 0 }, new[] { 0, 1 }, new[] { -1, 0 } };
            var lastRound = new List<int[]> { new[] { (int)Avatar.Location.X, (int)Avatar.Location.Y } };

            int distance = 3 + (int)Math.Log(Stats[Stat.Speed], 4);

            for (var i = 0; i < distance; i++)
            {
                var currentRound = new List<int[]>();

                foreach (var square in lastRound)
                {
                    foreach (var neighbor in neighbors)
                    {
                        // check if this cell has already been processed
                        if (grid.Weight[square[0] + neighbor[0], square[1] + neighbor[1]] == 1) continue;

                        var checkPoint = new Point(
                            square[0] + neighbor[0],
                            square[1] + neighbor[1]
                        );

                        if (checkPoint.X < 0 || checkPoint.X > battleboard.Size.Width || checkPoint.Y < 0 || checkPoint.Y > battleboard.Size.Height)
                            continue;

                        if (battleboard.Weight[checkPoint.X, checkPoint.Y] > 0)
                        {
                            currentRound.Add(new[] { square[0] + neighbor[0], square[1] + neighbor[1] });
                        }
                    }
                }

                foreach (var newSquare in currentRound)
                {
                    grid.Weight[newSquare[0], newSquare[1]] = 1;
                }

                lastRound = currentRound;
            }

            return grid;
        }
    }
}
