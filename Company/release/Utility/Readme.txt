2016. 11. 29

置段 持失

* class Util


	public Util();

        public static void ChangeImageFromTextrue2D(Texture2D tex, Image image);
        public static List<string> getJSonValueList(string str);
        public static string GetLocalIP();
        public static string getTimeString(int secondTime);
        public static bool SaveVideoData(WWW www);

* public class CustomWWW

        public CustomWWW();

        public static IEnumerator LoadData(Type type, DataForWWW data, ErrorHandler handler, DataHandler dataHandler);

        public enum Type
        {
            IMAGE = 0,
            VIDEO = 1,
            JSON = 2
        }

        public class DataForWWW
        {
            public WWWForm form;
            public string path;

            public DataForWWW(string path);
            public DataForWWW(string path, WWWForm form);
        }

        public delegate void DataHandler(WWW www);
        public delegate void ErrorHandler(string str);

* public class Log

        public Log();

        public static void d(string str);
        public static void e(string str);
        public static void i(string str);
        public static void w(string str);


2016. 12. 20

 - Changed Folder Tree.
 - Update Some Class.

 - Updated Class

* Class Util

	public Util();

        public static void ChangeImageFromTextrue2D(Texture2D tex, Image image);
        public static string getFileString(string path);
        public static string GetLocalIP();
        public static string getPriceWithComma(int i);
        public static string getPriceWithComma(string str);
        public static string getTimeString(string secondTime);
        public static string getTimeString(int secondTime);
        public static Dictionary<string, string> Parser(string path);
        public static void RemoveTitleOfWindow();
        public static bool SaveVideoData(WWW www, string filePath);

* Class CustomWWW

	public CustomWWW();

        public static IEnumerator LoadData(Type type, PostData data, ErrorHandler errorHandler, RequestHandler requestHandler);
        public static IEnumerator LoadData(Type type, PostData data, ProgressHandler progressHandler, ErrorHandler errorHandler, RequestHandler requestHandler);

        public enum Type
        {
            IMAGE = 0,
            VIDEO = 1,
            JSON = 2
        }

        public delegate void ErrorHandler(string str);
        public delegate void ProgressHandler(WWW www);
        public delegate void RequestHandler(WWW www);

* Class Log

        public Log();

        public static void d(string str);
        public static void d(string className, string str);
        public static void e(string str);
        public static void e(string className, string str);
        public static void i(string str);
        public static void i(string className, string str);
        public static void w(string str);
        public static void w(string className, string str);

 - New Class

* Class CommonServerAPI

        public CommonServerAPI();

        public void LoadImage(string url, CustomWWW.RequestHandler requestHandler);
        public void Login(IDictionary<string, string> data, CustomWWW.RequestHandler requestHandler, string loginUrl);

* Class Controller

        public IRefresh refresh;

        protected Controller();

        public abstract int Count { get; }
        public int Index { get; }

        public void indexChanged(int i);
        public void indexClear();
        public void indexDown();
        public void indexUp();
        public void setIRefresh(IRefresh i);

        public interface IRefresh
        {
            void refresh(int index);
        }

* Class Download

        public Download();

        public static Download Instance { get; }

        public void StartDownload(DownloadForm form, Finished finished, Progress progress);

        public class DownloadForm
        {
            public string downloadURL;
            public string savePath;

            public DownloadForm(string downloadURL, string savePath);
        }

        public delegate void Finished(bool isSaved);
        public delegate void Progress(int i);

* Class Hooking

        public Hooking();

        public static int CallNextHookEx(int idHook, int nCode, IntPtr wParam, IntPtr lParam);
        public static IntPtr hookProc(int code, IntPtr wParam, IntPtr lParam);
        public static int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);
        public static bool UnhookWindowsHookEx(int idHook);
        public void SetHook();
        public void SetInterfaceHooking(IHookingInterface i);
        public void UnHook();

        public interface IHookingInterface
        {
            MessageBase Left(int key);
            MessageBase Return(int key);
            MessageBase Right(int key);
        }

        public delegate int HookProc(int nCode, IntPtr wParam, IntPtr lParam);

* Class ListView

        [SerializeField]
        protected GameObject container;
        [SerializeField]
        protected GameObject item;

        protected ListView();

        public abstract int ItemCount { get; }
        public abstract int ItemHeight { get; }
        public abstract int StartYPosition { get; }

        public void Clear();
        public abstract void getItem(int index, GameObject obj);
        public void RefreshListViewNewItem();

* Class LoadImage

        public Image myImageComponent;

        public LoadImage();

        public void changedMainImage(WWW www);

* Class MainManager

        [SerializeField]
        protected Text duration;
        [SerializeField]
        protected LoadImage loadImage;
        [SerializeField]
        protected Text price;
        [SerializeField]
        protected Text titleText;

        protected MainManager();

        public abstract void refresh(int index);
        public virtual void Start();

* Class XMLController

        protected XMLController();

        public abstract string getSelectNodesPath { get; }
        public abstract string getXMLFilePath { get; }

        public abstract void FinishedReadXML(XmlNodeList list);
        public void setIFinishedReadXML(IFinishedReadXML i);
        public void xmlReader();
        protected void AddInnerText(XmlElement parent, string Element, string value);
        protected string getTitle(string value);
        protected void saveXML(XmlDocument doc, string path);

        public interface IFinishedReadXML
        {
            void FinishedReadXML();
        }

        public delegate void RequestRead(XmlNodeList list);

 - New DataForm

* Class MyMsgType

        public const short CheckingKeyValue = 48;
        public const short OnMonitoring = 49;

        public MyMsgType();

* Class PostData

        public WWWForm form;
        public string path;

        public PostData(string path);
        public PostData(string path, IDictionary<string, string> data);
        public PostData(string path, WWWForm form);

* Class Result

        public bool result;
        public string resultMsg;

        public Result();

* Class StringData

        public const string INIT_FILE_NAME = "init.ini";

        public StringData();

* Class VirtualKeyData

        public VirtualKeyData();

        public enum VKeys
        {
            VK_LBUTTON = 1,
            VK_RBUTTON = 2,
            VK_CANCEL = 3,
            VK_MBUTTON = 4,
            VK_BACK = 8,
            VK_TAB = 9,
            VK_CLEAR = 12,
            VK_RETURN = 13,
            VK_SHIFT = 16,
            VK_CONTROL = 17,
            VK_MENU = 18,
            VK_PAUSE = 19,
            VK_CAPITAL = 20,
            VK_HANGUL = 21,
            VK_ESCAPE = 27,
            VK_SPACE = 32,
            VK_PRIOR = 33,
            VK_NEXT = 34,
            VK_END = 35,
            VK_HOME = 36,
            VK_LEFT = 37,
            VK_UP = 38,
            VK_RIGHT = 39,
            VK_DOWN = 40,
            VK_SELECT = 41,
            VK_PRINT = 42,
            VK_EXECUTE = 43,
            VK_SNAPSHOT = 44,
            VK_INSERT = 45,
            VK_DELETE = 46,
            VK_HELP = 47,
            VK_0 = 48,
            VK_1 = 49,
            VK_2 = 50,
            VK_3 = 51,
            VK_4 = 52,
            VK_5 = 53,
            VK_6 = 54,
            VK_7 = 55,
            VK_8 = 56,
            VK_9 = 57,
            VK_A = 65,
            VK_B = 66,
            VK_C = 67,
            VK_D = 68,
            VK_E = 69,
            VK_F = 70,
            VK_G = 71,
            VK_H = 72,
            VK_I = 73,
            VK_J = 74,
            VK_K = 75,
            VK_L = 76,
            VK_M = 77,
            VK_N = 78,
            VK_O = 79,
            VK_P = 80,
            VK_Q = 81,
            VK_R = 82,
            VK_S = 83,
            VK_T = 84,
            VK_U = 85,
            VK_V = 86,
            VK_W = 87,
            VK_X = 88,
            VK_Y = 89,
            VK_Z = 90,
            VK_NUMPAD0 = 96,
            VK_NUMPAD1 = 97,
            VK_NUMPAD2 = 98,
            VK_NUMPAD3 = 99,
            VK_NUMPAD4 = 100,
            VK_NUMPAD5 = 101,
            VK_NUMPAD6 = 102,
            VK_NUMPAD7 = 103,
            VK_NUMPAD8 = 104,
            VK_NUMPAD9 = 105,
            VK_SEPARATOR = 108,
            VK_SUBTRACT = 109,
            VK_DECIMAL = 110,
            VK_DIVIDE = 111,
            VK_F1 = 112,
            VK_F2 = 113,
            VK_F3 = 114,
            VK_F4 = 115,
            VK_F5 = 116,
            VK_F6 = 117,
            VK_F7 = 118,
            VK_F8 = 119,
            VK_F9 = 120,
            VK_F10 = 121,
            VK_F11 = 122,
            VK_F12 = 123,
            VK_SCROLL = 145,
            VK_LSHIFT = 160,
            VK_RSHIFT = 161,
            VK_LCONTROL = 162,
            VK_RCONTROL = 163,
            VK_LMENU = 164,
            VK_RMENU = 165,
            VK_PLAY = 250,
            VK_ZOOM = 251
        }
