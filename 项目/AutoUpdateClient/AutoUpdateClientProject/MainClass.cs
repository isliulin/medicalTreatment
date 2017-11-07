using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MEDAS;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ToolAPI;

namespace AutoUpdateClient
{
    public class MainClass
    {
        Thread UpdateThread = null;
        public void Start()
        {
            UpdateThread = new Thread(Update);
            UpdateThread.Priority = ThreadPriority.Highest;
            UpdateThread.IsBackground = true;
            UpdateThread.Start();
            ToolAPI.XMLOperation.WriteLogXmlNoTail(Application.StartupPath + @"\UpdateLog\", "程序开启", "");
        }

        public void Stop()
        {
            try
            {
                UpdateThread.Abort();
                ToolAPI.XMLOperation.WriteLogXmlNoTail(Application.StartupPath + @"\UpdateLog\", "程序关闭", "");
            }
            catch (Exception ex) { ToolAPI.XMLOperation.WriteLogXmlNoTail(Application.StartupPath + @"\UpdateLog\", "程序关闭异常", ex.Message + ex.StackTrace); }

        }

        private void Update()
        {
            while (true)
            {
                if (DateTime.Now.Hour == 12 && DateTime.Now.Minute <= 10)
                {
                    try
                    {
                        //获取版本更新 得到对应的版本号和对应的地址
                        //{\"code\":200,\"data\":{\"version\":\"1.0\",\"url\":\"xxxxxxxxxxxx\"},\"message\":\"\\u64cd\\u4f5c\\u6210\\u529f\"}
                        string result = DataUpload.Version();
                        string[] stringSeparators = new string[] { "{", "," };
                        string[] trem = result.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                        if (result.Contains("\"code\":200"))
                        {
                            string version = "";
                            string url = "";
                            foreach (string tremTemp in trem)
                            {
                                string[] term1 = tremTemp.Split(':');
                                if (tremTemp.Contains("version"))
                                    version = term1.Length >= 2 ? term1[1] : "";
                                else if (tremTemp.Contains("url"))
                                    url = term1.Length >= 2 ? term1[1] : "";
                            }
                            version = version.Replace("\"", "");
                            string localVersion = INIOperate.IniReadValue("Base", "version", Application.StartupPath + "\\Config.ini");
                            if (version != localVersion) //开始更新
                            {
                                if (!string.IsNullOrEmpty(url))
                                {
                                    try
                                    {
                                        url = "http://tcmh.zy360.com/tcmd/" + url.Replace("\"", "").Replace("}", "");
                                        //url = "https://zhidao.baidu.com/question/521271883195213765.html"; //测试用
                                        string[] urlTrem = url.Split('/');
                                        string LocalPath = Application.StartupPath + "\\AUTemporary\\" + urlTrem[urlTrem.Length - 1];
                                        bool isSuc = FileDownload(url, LocalPath);
                                        // 执行一次服务关闭
                                        DateTime dt = DateTime.Now;
                                        string ServiceName = ToolAPI.INIOperate.IniReadValue("WindowsServer", "ServiceName", Application.StartupPath + "\\Config.ini");
                                        bool isStop = WinServer.StopService(ServiceName);
                                        ToolAPI.XMLOperation.WriteLogXmlNoTail(Application.StartupPath + @"\UpdateLog\", "服务被关闭", isStop.ToString() + ";" + ServiceName);
                                        if (isStop)
                                        {
                                            try
                                            {
                                                //ZipHelper.UnZip(Application.StartupPath + "\\AUTemporary\\" + "20170706.zip", Application.StartupPath);//测试用
                                                ZipHelper.UnZip(LocalPath, Application.StartupPath);
                                                ToolAPI.XMLOperation.WriteLogXmlNoTail(Application.StartupPath + @"\UpdateLog\", "更新结束", isStop.ToString() + ";" + (DateTime.Now - dt).TotalMilliseconds.ToString());
                                                //更新完毕后要把配置文件中的版本号更改喽
                                                INIOperate.IniWriteValue("Base", "version", version, Application.StartupPath + "\\Config.ini");
                                                ToolAPI.XMLOperation.WriteLogXmlNoTail(Application.StartupPath + @"\UpdateLog\", "更新结束后把配置文件版本到最新", version);
                                            }
                                            catch (Exception ex)
                                            {
                                                ToolAPI.XMLOperation.WriteLogXmlNoTail(Application.StartupPath + @"\UpdateLog\", "解压拷贝出现异常", ex.Message);
                                            }
                                            //执行一次开启
                                            bool isStart = WinServer.StartService(ServiceName);
                                            ToolAPI.XMLOperation.WriteLogXmlNoTail(Application.StartupPath + @"\UpdateLog\", "服务再次开启", isStart.ToString() + ";" + ServiceName);
                                        }
                                        File.Delete(LocalPath);
                                    }
                                    catch (Exception ex)
                                    {
                                        ToolAPI.XMLOperation.WriteLogXmlNoTail(Application.StartupPath + @"\UpdateLog\", "执行更新出现异常", ex.Message);
                                    }
                                }
                            }
                            else
                            {
                                ToolAPI.XMLOperation.WriteLogXmlNoTail(Application.StartupPath + @"\UpdateLog\", "执行更新出现异常", version + ";" + localVersion);
                            }
                        }
                        else
                        {
                            ToolAPI.XMLOperation.WriteLogXmlNoTail(Application.StartupPath + @"\UpdateLog\", "获取版本号出错", JsonConvert.DeserializeObject(result).ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        ToolAPI.XMLOperation.WriteLogXmlNoTail(Application.StartupPath + @"\UpdateLog\", "版本号相同不存在更新", ex.Message);
                    }
                }
                else
                {
                    ToolAPI.XMLOperation.WriteLogXmlNoTail(Application.StartupPath + @"\UpdateLog\", "当前时间不符合", DateTime.Now.ToString());
                }
                //Thread.Sleep(300000);//测试用
                Thread.Sleep(540000);//10分钟监测一次
            }
        }


        #region 文件下载
        bool FileDownload(string ServerPath, string LocalPath)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ServerPath);
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                Stream responseStream = response.GetResponseStream();
                Stream stream = new FileStream(LocalPath, FileMode.Create);
                byte[] bArr = new byte[1024];
                int size = responseStream.Read(bArr, 0, bArr.Length);
                while (size > 0)
                {
                    stream.Write(bArr, 0, size);
                    size = responseStream.Read(bArr, 0, bArr.Length);
                }
                stream.Close();
                responseStream.Close();
                return true;
            }
            catch (Exception ex)
            {
                ToolAPI.XMLOperation.WriteLogXmlNoTail(Application.StartupPath + @"\UpdateLog\", "文件下载出现异常", ServerPath + ";" + LocalPath+";"+ex.Message);
                return false;
            }
        }
        #endregion
    }
}
