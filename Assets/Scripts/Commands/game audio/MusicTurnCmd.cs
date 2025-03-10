using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using ViewModel;
using Controllers;
using Infrastructure;
using System;

namespace Commands
{
    public class MusicTurnCmd : ICommand
    {
        private GameSound gameSound;
        private bool isOn;

       
        public MusicTurnCmd(GameSound gameSound, bool isOn)
        {
            this.gameSound = gameSound;  
            this.isOn = isOn;
        }

        public void Execute()
        {
            if (gameSound == null)
            {
                Debug.LogError("gameSound is NULL!");
                return;
            }

            if (gameSound.isMusicOn == null)
            {
                Debug.LogError("gameSound.isMusicOn is NULL!");
                return;
            }

            gameSound.isMusicOn.Value = isOn;
        }
    }
}
