using UnityEngine;

namespace Assets.Scripts
{
    public class Gui : MonoBehaviour
    {
        public StatsCounter statsCounter;

        public Texture2D customGuiTexture;
        public Texture2D customButtonTexture;

        private Rect menuRect = new Rect(Screen.width / 2 - 100, 10, 200, 190);
        private Rect statsRect = new Rect(Screen.width - 210, 10, 200, 210);

        private bool menuOpen = false;
        private bool statsOpen = false;


        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                menuOpen = !menuOpen;
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                statsOpen = !statsOpen;
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                Screen.fullScreen = !Screen.fullScreen;
            }
        }

        void MenuWindow(int windowID)
        {
            int spacing = 10;
            int height = 25;
            int offset = 20;
            int width = 200 - (offset * 2);

            if (GUI.Button(new Rect(offset, height * 1 + spacing * 1, width, height), "Close menu (Esc)"))
            {
                menuOpen = !menuOpen;
            }

            if (GUI.Button(new Rect(offset, height * 2 + spacing * 2, width, height), "Fullscreen (F)"))
            {
                Screen.fullScreen = !Screen.fullScreen;
            }

            if (GUI.Button(new Rect(offset, height * 3 + spacing * 3, width, height), "Enable stats (S)"))
            {
                statsOpen = !statsOpen;
                GUI.BringWindowToFront(0);
            }

            if (GUI.Button(new Rect(offset, height * 4 + spacing * 4, width, height), "Quit"))
            {
                Application.Quit();
            }
        }

        void StatsWindow(int windowID) 
        {
            int spacing = 0;
            int height = 25;
            int offset = 20;
            int width = 200 - (offset * 2);

            GUI.Label(new Rect(offset, height * 1 + spacing * 1, width, height), "Moves left: " + statsCounter.MovesLeft);
            GUI.Label(new Rect(offset, height * 2 + spacing * 2, width, height), "Moves right: " + statsCounter.MovesRight);
            GUI.Label(new Rect(offset, height * 3 + spacing * 3, width, height), "Blocks grabbed: " + statsCounter.Grabs);
            GUI.Label(new Rect(offset, height * 4 + spacing * 4, width, height), "Failed grabs: " + statsCounter.PretendGrabs);
            GUI.Label(new Rect(offset, height * 5 + spacing * 5, width, height), "Blocks dropped: " + statsCounter.Drops);
            GUI.Label(new Rect(offset, height * 6 + spacing * 6, width, height), "Failed drops: " + statsCounter.PretendDrops);
            GUI.Label(new Rect(offset, height * 7 + spacing * 7, width, height), "Blocks scanned: " + statsCounter.Scans);
        }

        void OnGUI()
        {
            if (menuOpen || statsOpen)
            {
                GUI.skin.box.normal.background = customGuiTexture;

                GUI.skin.window.active.background = customGuiTexture;

                GUI.skin.button.normal.background = customButtonTexture;
                GUI.skin.button.focused.background = customButtonTexture;
                GUI.skin.button.hover.background = customButtonTexture;
                GUI.skin.button.active.background = customButtonTexture;
            }


            if (menuOpen)
            {
                menuRect = GUI.Window(0, menuRect, MenuWindow, "Menu");
            }

            if (statsOpen)
            {
                statsRect = GUI.Window(1, statsRect, StatsWindow, "Stats");
            }
        }
    }
}