﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls;
using Nuclex.UserInterface.Controls.Desktop;

namespace SRPG.Scene.PartyMenu
{
    public partial class CharacterStatsDialog : WindowControl
    {
        private LabelControl _defLabel;
        private LabelControl _defLevel;
        private ProgressControl _defProg;
        private LabelControl _defExp;

        private LabelControl _attLabel;
        private LabelControl _attLevel;
        private ProgressControl _attProg;
        private LabelControl _attExp;

        private LabelControl _wisLabel;
        private LabelControl _wisLevel;
        private ProgressControl _wisProg;
        private LabelControl _wisExp;

        private LabelControl _intLabel;
        private LabelControl _intLevel;
        private ProgressControl _intProg;
        private LabelControl _intExp;

        private LabelControl _spdLabel;
        private LabelControl _spdLevel;
        private ProgressControl _spdProg;
        private LabelControl _spdExp;

        private LabelControl _hitLabel;
        private LabelControl _hitLevel;
        private ProgressControl _hitProg;
        private LabelControl _hitExp;

        private void InitializeComponent()
        {
            EnableDragging = false;

            // number of abilities to show (DAWISH)
            // leaving this dynamic incase i change the count and don't
            // want to recalculate all numbers
            var n = 6f;

            //
            // defLabel
            //
            _defLabel = new LabelControl();
            _defLabel.Bounds = new UniRectangle(
                new UniScalar(10), new UniScalar(1 / (2 * n), -12.5f),
                new UniScalar(0.0F, 50.0F), new UniScalar(0.0F, 25.0F)
                );
            _defLabel.Text = "DEF";
            Children.Add(_defLabel);

            //
            // defText
            //
            _defLevel = new LabelControl();
            _defLevel.Bounds = new UniRectangle(
                new UniScalar(0, 60), new UniScalar(1 / (2 * n), -12.5f),
                new UniScalar(0.0F, 40.0F), new UniScalar(0.0F, 25.0F)
                );
            Children.Add(_defLevel);

            //
            // defProg
            //
            _defProg = new ProgressControl();
            _defProg.Bounds = new UniRectangle(
                new UniScalar(0, 100), new UniScalar(0, 5),
                new UniScalar(1.0F, -150.0F), new UniScalar(1f / n, -10.0F)
            );
            _defProg.Progress = 0.5f;
            Children.Add(_defProg);

            //
            // defLevel
            //
            _defExp = new LabelControl();
            _defExp.Bounds = new UniRectangle(
                new UniScalar(1, -35), new UniScalar(1 / (2 * n), -12.5f),
                new UniScalar(0.0F, 25.0F), new UniScalar(0.0F, 25.0F)
                );
            _defExp.Text = "1";
            Children.Add(_defExp);

            //
            // attLabel
            //
            _attLabel = new LabelControl();
            _attLabel.Bounds = new UniRectangle(
                new UniScalar(10), new UniScalar(3 / (2 * n), -12.5f),
                new UniScalar(0.0F, 50.0F), new UniScalar(0.0F, 25.0F)
                );
            _attLabel.Text = "ATT";
            Children.Add(_attLabel);

            //
            // attText
            //
            _attLevel = new LabelControl();
            _attLevel.Bounds = new UniRectangle(
                new UniScalar(0, 60), new UniScalar(3 / (2 * n), -12.5f),
                new UniScalar(0.0F, 40.0F), new UniScalar(0.0F, 25.0F)
                );
            Children.Add(_attLevel);

            //
            // attProg
            //
            _attProg = new ProgressControl();
            _attProg.Bounds = new UniRectangle(
                new UniScalar(0, 100), new UniScalar(1f / n, 5),
                new UniScalar(1.0F, -150.0F), new UniScalar(1f / n, -10.0F)
            );
            _attProg.Progress = 0.5f;
            Children.Add(_attProg);

            //
            // attLevel
            //
            _attExp = new LabelControl();
            _attExp.Bounds = new UniRectangle(
                new UniScalar(1, -35), new UniScalar(3 / (2 * n), -12.5f),
                new UniScalar(0.0F, 25.0F), new UniScalar(0.0F, 25.0F)
                );
            _attExp.Text = "1";
            Children.Add(_attExp);

            //
            // wisLabel
            //
            _wisLabel = new LabelControl();
            _wisLabel.Bounds = new UniRectangle(
                new UniScalar(10), new UniScalar(5 / (2 * n), -12.5f),
                new UniScalar(0.0F, 50.0F), new UniScalar(0.0F, 25.0F)
                );
            _wisLabel.Text = "WIS";
            Children.Add(_wisLabel);

            //
            // wisText
            //
            _wisLevel = new LabelControl();
            _wisLevel.Bounds = new UniRectangle(
                new UniScalar(0, 60), new UniScalar(5 / (2 * n), -12.5f),
                new UniScalar(0.0F, 40.0F), new UniScalar(0.0F, 25.0F)
                );
            Children.Add(_wisLevel);

            //
            // wisProg
            //
            _wisProg = new ProgressControl();
            _wisProg.Bounds = new UniRectangle(
                new UniScalar(0, 100), new UniScalar(2f / n, 5),
                new UniScalar(1.0F, -150.0F), new UniScalar(1f / n, -10.0F)
            );
            _wisProg.Progress = 0.5f;
            Children.Add(_wisProg);

            //
            // wisLevel
            //
            _wisExp = new LabelControl();
            _wisExp.Bounds = new UniRectangle(
                new UniScalar(1, -35), new UniScalar(5 / (2 * n), -12.5f),
                new UniScalar(0.0F, 25.0F), new UniScalar(0.0F, 25.0F)
                );
            _wisExp.Text = "1";
            Children.Add(_wisExp);

            //
            // intLabel
            //
            _intLabel = new LabelControl();
            _intLabel.Bounds = new UniRectangle(
                new UniScalar(10), new UniScalar(7 / (2 * n), -12.5f),
                new UniScalar(0.0F, 50.0F), new UniScalar(0.0F, 25.0F)
                );
            _intLabel.Text = "INT";
            Children.Add(_intLabel);

            //
            // intText
            //
            _intLevel = new LabelControl();
            _intLevel.Bounds = new UniRectangle(
                new UniScalar(0, 60), new UniScalar(7 / (2 * n), -12.5f),
                new UniScalar(0.0F, 40.0F), new UniScalar(0.0F, 25.0F)
                );
            Children.Add(_intLevel);

            //
            // intProg
            //
            _intProg = new ProgressControl();
            _intProg.Bounds = new UniRectangle(
                new UniScalar(0, 100), new UniScalar(3f / n, 5),
                new UniScalar(1.0F, -150.0F), new UniScalar(1f / n, -10.0F)
            );
            _intProg.Progress = 0.5f;
            Children.Add(_intProg);

            //
            // intLevel
            //
            _intExp = new LabelControl();
            _intExp.Bounds = new UniRectangle(
                new UniScalar(1, -35), new UniScalar(7 / (2 * n), -12.5f),
                new UniScalar(0.0F, 25.0F), new UniScalar(0.0F, 25.0F)
                );
            _intExp.Text = "1";
            Children.Add(_intExp);

            //
            // spdLabel
            //
            _spdLabel = new LabelControl();
            _spdLabel.Bounds = new UniRectangle(
                new UniScalar(10), new UniScalar(9 / (2 * n), -12.5f),
                new UniScalar(0.0F, 50.0F), new UniScalar(0.0F, 25.0F)
                );
            _spdLabel.Text = "SPD";
            Children.Add(_spdLabel);

            //
            // spdText
            //
            _spdLevel = new LabelControl();
            _spdLevel.Bounds = new UniRectangle(
                new UniScalar(0, 60), new UniScalar(9 / (2 * n), -12.5f),
                new UniScalar(0.0F, 40.0F), new UniScalar(0.0F, 25.0F)
                );
            Children.Add(_spdLevel);

            //
            // spdProg
            //
            _spdProg = new ProgressControl();
            _spdProg.Bounds = new UniRectangle(
                new UniScalar(0, 100), new UniScalar(4f / n, 5),
                new UniScalar(1.0F, -150.0F), new UniScalar(1f / n, -10.0F)
            );
            _spdProg.Progress = 0.5f;
            Children.Add(_spdProg);

            //
            // spdLevel
            //
            _spdExp = new LabelControl();
            _spdExp.Bounds = new UniRectangle(
                new UniScalar(1, -35), new UniScalar(9 / (2 * n), -12.5f),
                new UniScalar(0.0F, 25.0F), new UniScalar(0.0F, 25.0F)
                );
            _spdExp.Text = "1";
            Children.Add(_spdExp);

            //
            // hitLabel
            //
            _hitLabel = new LabelControl();
            _hitLabel.Bounds = new UniRectangle(
                new UniScalar(10), new UniScalar(11 / (2 * n), -12.5f),
                new UniScalar(0.0F, 50.0F), new UniScalar(0.0F, 25.0F)
                );
            _hitLabel.Text = "HIT";
            Children.Add(_hitLabel);

            //
            // hitText
            //
            _hitLevel = new LabelControl();
            _hitLevel.Bounds = new UniRectangle(
                new UniScalar(0, 60), new UniScalar(11 / (2 * n), -12.5f),
                new UniScalar(0.0F, 40.0F), new UniScalar(0.0F, 13.0F)
                );
            Children.Add(_hitLevel);

            //
            // hitProg
            //
            _hitProg = new ProgressControl();
            _hitProg.Bounds = new UniRectangle(
                new UniScalar(0, 100), new UniScalar(5f / n, 5),
                new UniScalar(1.0F, -150.0F), new UniScalar(1f / n, -10.0F)
            );
            _hitProg.Progress = 0.5f;
            Children.Add(_hitProg);

            //
            // hitLevel
            //
            _hitExp = new LabelControl();
            _hitExp.Bounds = new UniRectangle(
                new UniScalar(1, -35), new UniScalar(11 / (2 * n), -12.5f),
                new UniScalar(0.0F, 25.0F), new UniScalar(0.0F, 25.0F)
                );
            _hitExp.Text = "1";
            Children.Add(_hitExp);
        }
    }
}
