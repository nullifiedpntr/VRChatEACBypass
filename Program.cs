using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace VRChatEmulator
{
    class Program
    {

        static string revert = @"{
	""productid""										: ""a4a57ff548934dbeba0cc7c62cdf9f34"",
	""sandboxid""										: ""198988a88fe34e3e91ed15f7c91a15b1"",
	""deploymentid""                                  : ""76151011566a4757ae955b380b05c378"",
	""title""											: ""VRChat"",
	""executable""									: ""VRChat.exe"",
	""requested_splash""								: ""EasyAntiCheat/SplashScreen.png"",
	""wait_for_game_process_exit""					: ""false""
}";

        static string patched = @"{
	""productid""										: ""a4a57ff548934dbeba0cc7c62cdf9f31"",
	""sandboxid""										: ""198988a88fe34e3e91ed15f7c91a15b9"",
	""deploymentid""                                  : ""76151011566a4757ae955b380b05c373"",
	""title""											: ""VRChat"",
	""executable""									: ""VRChat.exe"",
	""requested_splash""								: ""EasyAntiCheat/SplashScreen.png"",
	""wait_for_game_process_exit""					: ""false""
}";

        static bool handle = false;

        public static void updateColor(ConsoleColor color)
        {
            Console.ForegroundColor = color;
        }

        public static void Main(string[] args) 
        {
            updateColor(ConsoleColor.DarkBlue);
            Console.WriteLine(@"__     ______   ____ _           _           
\ \   / /  _ \ / ___| |__   __ _| |_         
 \ \ / /| |_) | |   | '_ \ / _` | __|        
  \ V / |  _ <| |___| | | | (_| | |_         
 __\_/  |_| \_\\____|_|_|_|\__,_|\__|        
| ____|_ __ ___  _   _| | __ _| |_ ___  _ __ 
|  _| | '_ ` _ \| | | | |/ _` | __/ _ \| '__|
| |___| | | | | | |_| | | (_| | || (_) | |   
|_____|_| |_| |_|\__,_|_|\__,_|\__\___/|_|   ");
            Console.WriteLine(" ");
            // TODO: Change the path to your own 
            string path = @"G:\SteamLibrary\steamapps\common\VRChat\EasyAntiCheat\Settings.json";
            try
            {
                patch(path);
                updateColor(ConsoleColor.Green);
                Console.WriteLine("[+] Patched Settings.json");
            }
            catch (Exception e) { updateColor(ConsoleColor.Red); Console.WriteLine("[-] Exception: " + e.Message); Console.WriteLine("[-] Failed patching Settings.json! Terminating..."); Thread.Sleep(1000); Environment.Exit(0); }

            updateColor(ConsoleColor.Cyan);
            Console.WriteLine("[+] Waiting for game...");
            while (true)
            {

                if (Process.GetProcessesByName("start_protected_game").Length > 0) 
                {
                    var process = Process.GetProcessesByName("start_protected_game")[0];
                    if (!handle)
                    {
                        handle = true;
                        updateColor(ConsoleColor.Magenta);
                        Console.WriteLine("[+] Got EAC Handle!");
                    }    
                    if (process.HasExited)
                    {
                        updateColor(ConsoleColor.Green);
                        Console.WriteLine("[+] Game loading");
                        try
                        {
                            original(path);
                            Console.WriteLine("[-] Reverted Settings.json to original state");
                        } catch (Exception e) { updateColor(ConsoleColor.Red); Console.WriteLine("[-] Exception: " + e.Message); Console.WriteLine("[-] Failed reverting Settings.json to original state! Terminating..."); Thread.Sleep(1000); Environment.Exit(0); }

                        updateColor(ConsoleColor.Blue);
                        Console.WriteLine("{+] EAC Bypassed");
                        break;
                    }
                }


            }
            

        }

        public static void patch(string pathToSettings)
        {
            File.WriteAllText(pathToSettings, patched);
        }

        public static void original(string pathToSettings)
        {
            File.WriteAllText(pathToSettings, revert);
        }
    }
}