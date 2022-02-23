/*
* (C) Copyright 2014, Valerian Gaudeau
* 
* Kerbal Space Program is Copyright (C) 2013 Squad. See http://kerbalspaceprogram.com/. This
* project is in no way associated with nor endorsed by Squad.
* 
* This code is licensed under the Apache License Version 2.0. See the LICENSE.txt
* file for more information.
*/

using KSP.UI.Screens;
using PlanetShine.Utils;
using UnityEngine;

namespace PlanetShine.Gui
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class GuiManager : MonoBehaviour
    {
        public bool IsConfigDisplayed
        {
            get
            {
                return isConfigDisplayed;
            }

            set
            {
                if (isConfigDisplayed == value)
                    return;
                isConfigDisplayed = value;
                UpdateButtonIcons();
            }
        }
        private bool isConfigDisplayed = false;

        private Config config = Config.Instance;
        private PlanetShine planetShine;
        private IButton blizzyButton;
        private ApplicationLauncherButton stockButton;
        private GuiRenderer guiRenderer;

        public void Start()
        {
            guiRenderer = new GuiRenderer(this);
            UpdateToolbarBlizzy();
            UpdateToolbarStock();
        }

        public void UpdateToolbarStock()
        {
            if (stockButton != null)
            {
                if (!config.stockToolbarEnabled && config.blizzyToolbarInstalled)
                {
                    ApplicationLauncher.Instance.RemoveModApplication(stockButton);
                    stockButton = null;
                }
                return;
            }
            else if (!config.stockToolbarEnabled && config.blizzyToolbarInstalled)
                return;
            stockButton = ApplicationLauncher.Instance.AddModApplication(
                () =>
                {
                    planetShine = PlanetShine.Instance;
                    IsConfigDisplayed = true;
                },
                () =>
                {
                    IsConfigDisplayed = false;
                },
                null,
                null,
                null,
                null,
                ApplicationLauncher.AppScenes.FLIGHT,
                GameDatabase.Instance.GetTexture("PlanetShine/Icons/ps_toolbar", false)
                );
            if (IsConfigDisplayed)
                stockButton.SetTrue();
        }

        public void UpdateToolbarBlizzy()
        {
            if (!config.blizzyToolbarInstalled)
                return;
            if (blizzyButton != null)
                return;
            blizzyButton = ToolbarManager.Instance.add("PlanetShine", "Gui");
            blizzyButton.TexturePath = "PlanetShine/Icons/ps_disabled";
            blizzyButton.Visibility = new GameScenesVisibility(GameScenes.FLIGHT);
            blizzyButton.ToolTip = "PlanetShine Settings";
            blizzyButton.OnClick += (e) =>
            {
                planetShine = PlanetShine.Instance;
                IsConfigDisplayed = !IsConfigDisplayed;
            };
        }

        private void UpdateButtonIcons() {
            if (blizzyButton != null)
                blizzyButton.TexturePath = IsConfigDisplayed ? "PlanetShine/Icons/ps_enabled" : "PlanetShine/Icons/ps_disabled";
            if (stockButton != null)
                if (isConfigDisplayed)
                    stockButton.SetTrue();
                else
                    stockButton.SetFalse();       
        }

        private void OnGUI(){
            if (IsConfigDisplayed) {
                guiRenderer.Render(planetShine);
            }
        }
        
        private void OnDestroy() {
            if (stockButton != null)
                ApplicationLauncher.Instance.RemoveModApplication(stockButton);
            if (blizzyButton != null)
                blizzyButton.Destroy();
        }
    }
}

