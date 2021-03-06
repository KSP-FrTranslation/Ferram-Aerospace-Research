﻿/*
Ferram Aerospace Research v0.15.9.5 "Lighthill"
=========================
Aerodynamics model for Kerbal Space Program

Copyright 2017, Michael Ferrara, aka Ferram4

   This file is part of Ferram Aerospace Research.

   Ferram Aerospace Research is free software: you can redistribute it and/or modify
   it under the terms of the GNU General Public License as published by
   the Free Software Foundation, either version 3 of the License, or
   (at your option) any later version.

   Ferram Aerospace Research is distributed in the hope that it will be useful,
   but WITHOUT ANY WARRANTY; without even the implied warranty of
   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
   GNU General Public License for more details.

   You should have received a copy of the GNU General Public License
   along with Ferram Aerospace Research.  If not, see <http://www.gnu.org/licenses/>.

   Serious thanks:		a.g., for tons of bugfixes and code-refactorings
				stupid_chris, for the RealChuteLite implementation
            			Taverius, for correcting a ton of incorrect values
				Tetryds, for finding lots of bugs and issues and not letting me get away with them, and work on example crafts
            			sarbian, for refactoring code for working with MechJeb, and the Module Manager updates
            			ialdabaoth (who is awesome), who originally created Module Manager
                        	Regex, for adding RPM support
				DaMichel, for some ferramGraph updates and some control surface-related features
            			Duxwing, for copy editing the readme

   CompatibilityChecker by Majiir, BSD 2-clause http://opensource.org/licenses/BSD-2-Clause

   Part.cfg changes powered by sarbian & ialdabaoth's ModuleManager plugin; used with permission
	http://forum.kerbalspaceprogram.com/threads/55219

   ModularFLightIntegrator by Sarbian, Starwaster and Ferram4, MIT: http://opensource.org/licenses/MIT
	http://forum.kerbalspaceprogram.com/threads/118088

   Toolbar integration powered by blizzy78's Toolbar plugin; used with permission
	http://forum.kerbalspaceprogram.com/threads/60863
 */

using System;
using System.Collections.Generic;
using UnityEngine;
using FerramAerospaceResearch.FARUtils;

namespace FerramAerospaceResearch.FARThreading
{
    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    class ThreadSafeDebugLogger : MonoBehaviour
    {
        static ThreadSafeDebugLogger _instance;
        public static ThreadSafeDebugLogger Instance
        {
            get { return _instance; }
        }

        List<Exception> _exceptionsThrown;
        List<string> _debugMessages;

        void Awake()
        {
            _instance = this;
            _exceptionsThrown = new List<Exception>();
            _debugMessages = new List<string>();
            GameObject.DontDestroyOnLoad(this.gameObject);
        }

        void Update()
        {
            if (_exceptionsThrown.Count > 0)
            {
                for (int i = 0; i < _exceptionsThrown.Count; i++)
                    FARLogger.Exception(_exceptionsThrown[i]);

                _exceptionsThrown.Clear();
            }


            if (_debugMessages.Count > 0)
            {
                System.Text.StringBuilder sB = new System.Text.StringBuilder();
                for (int i = 0; i < _debugMessages.Count; i++)
                    sB.AppendLine(_debugMessages[i]);

                _debugMessages.Clear();

                FARLogger.Info("" + sB.ToString());
            }

        }

        public void RegisterMessage(string s)
        {
            _debugMessages.Add(s);
        }

        public void RegisterException(Exception e)
        {
            _exceptionsThrown.Add(e);
        }
    }
}
