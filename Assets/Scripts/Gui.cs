using UnityEngine;

namespace Assets.Scripts
{
    public class Gui : MonoBehaviour
    {
        public StatsCounter statsCounter;

        public Texture2D customGuiTexture;
        public Texture2D customButtonTexture;

        void Start()
        {
            _perspectiveSwitcher = Camera.main.GetComponent<PerspectiveSwitcher>();
        }

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

            int btnAmount = 5;
            Rect[] rects = new Rect[btnAmount];
            for (int i = 0; i < btnAmount; i++)
            {
                rects[i] = new Rect(offset, height * (i+1) + spacing * (i+1), width, height);
            }

            if (GUI.Button(rects[0], "Close menu (Esc)"))
            {
                menuOpen = !menuOpen;
            }

            if (GUI.Button(rects[1], "Fullscreen (F)"))
            {
                Screen.fullScreen = !Screen.fullScreen;
            }

            if (GUI.Button(rects[2], "Enable stats (S)"))
            {
                statsOpen = !statsOpen;
                GUI.BringWindowToFront(0);
            }

            if (GUI.Button(rects[3], "3D"))
            {
                _perspectiveSwitcher.Switch();
            }

            if (GUI.Button(rects[4], "Quit"))
            {
                Application.Quit();
            }
        }

        void StatsWindow(int windowID) 
        {
            scrollPositionStats = GUILayout.BeginScrollView(scrollPositionStats);

            // total
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Executed commands: ");
                GUI.skin.GetStyle("Label").alignment = TextAnchor.UpperRight;
                GUILayout.Label("" + statsCounter.Total);
                GUI.skin.GetStyle("Label").alignment = TextAnchor.UpperLeft;
            }
            GUILayout.EndHorizontal();

            // queued
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Queued commands: ");
                GUI.skin.GetStyle("Label").alignment = TextAnchor.UpperRight;
                GUILayout.Label("" + statsCounter.Queued);
                GUI.skin.GetStyle("Label").alignment = TextAnchor.UpperLeft;
            }
            GUILayout.EndHorizontal();

            // left
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Moves left: ");
                GUI.skin.GetStyle("Label").alignment = TextAnchor.UpperRight;
                GUILayout.Label("" + statsCounter.MovesLeft);
                GUI.skin.GetStyle("Label").alignment = TextAnchor.UpperLeft;
            }
            GUILayout.EndHorizontal();

            // right
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Moves right: ");
                GUI.skin.GetStyle("Label").alignment = TextAnchor.UpperRight;
                GUILayout.Label("" + statsCounter.MovesRight);
                GUI.skin.GetStyle("Label").alignment = TextAnchor.UpperLeft;
            }
            GUILayout.EndHorizontal();

            // grabs
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Blocks grabbed: ");
                GUI.skin.GetStyle("Label").alignment = TextAnchor.UpperRight;
                GUILayout.Label("" + statsCounter.Grabs);
                GUI.skin.GetStyle("Label").alignment = TextAnchor.UpperLeft;
            }
            GUILayout.EndHorizontal();

            // failed grabs
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Failed grabs: ");
                GUI.skin.GetStyle("Label").alignment = TextAnchor.UpperRight;
                GUILayout.Label("" + statsCounter.PretendGrabs);
                GUI.skin.GetStyle("Label").alignment = TextAnchor.UpperLeft;
            }
            GUILayout.EndHorizontal();

            // drops
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Blocks dropped: ");
                GUI.skin.GetStyle("Label").alignment = TextAnchor.UpperRight;
                GUILayout.Label("" + statsCounter.Drops);
                GUI.skin.GetStyle("Label").alignment = TextAnchor.UpperLeft;
            }
            GUILayout.EndHorizontal();

            // failed drops
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Failed drops: ");
                GUI.skin.GetStyle("Label").alignment = TextAnchor.UpperRight;
                GUILayout.Label("" + statsCounter.PretendDrops);
                GUI.skin.GetStyle("Label").alignment = TextAnchor.UpperLeft;
            }
            GUILayout.EndHorizontal();

            // scan
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Blocks scanned: ");
                GUI.skin.GetStyle("Label").alignment = TextAnchor.UpperRight;
                GUILayout.Label("" + statsCounter.Scans);
                GUI.skin.GetStyle("Label").alignment = TextAnchor.UpperLeft;
            }
            GUILayout.EndHorizontal();


            GUILayout.EndScrollView();
        }

        void OnGUI()
        {
            if (menuOpen || statsOpen)
            {
                GUI.skin.window.normal.background = customGuiTexture;
                GUI.skin.window.active.background = customGuiTexture;
                GUI.skin.window.focused.background = customGuiTexture;
                GUI.skin.window.hover.textColor = Color.white;

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


        // these Rect's controls the size and position of the menus
        private Rect menuRect = new Rect(Screen.width / 2 - 100, 10, 200, 220);
        private Rect statsRect = new Rect(Screen.width - 260, 10, 250, 190);

        private Vector2 scrollPositionStats = Vector2.zero;
        private PerspectiveSwitcher _perspectiveSwitcher;

        private bool menuOpen = false;
        private bool statsOpen = false;
    }
}