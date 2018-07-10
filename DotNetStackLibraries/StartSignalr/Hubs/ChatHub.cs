using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace StartSignalr.Hubs
{
    /// <summary>
    /// 方法重载时必须参数个数不同
    /// </summary>
    [HubName("chatHub")]
    public class ChatHub : Hub
    {
        [HubMethodName("send")]
        public void Send(string name, string message)
        {
            //所有客户端
            //调用客户端方法是大小写 不敏感 的
            Clients.All.AddNewMessageToPage(name, message);

            //效果同上
            //string methodToCall = "AddNewMessageToPage";
            //(Clients.All as IClientProxy).Invoke(methodToCall, name, message);

            //调用者
            //Clients.Caller

            //除调用者以外的
            //Clients.Others

            //特定ConnectionId的
            //Clients.Client(Context.ConnectionId)

            //指定userName的
            //Clients.Client(userName);

            //特定ConnectionId列表的
            //Clients.Clients(ConnectionIdList);

            //除了指定ConnectionId以外的
            //var connectionId1 = Context.ConnectionId;
            //var connectionId2 = Context.ConnectionId;
            //Clients.AllExcept(connectionId1, connectionId2);

            //指定组的
            //var groupName = "Admin";
            //Clients.Group(groupName)

            //指定组列表的
            //Clients.Groups(groupNameList);

            //指定组的，去除特定的ConnectionId
            //Clients.Group(groupName, connectionId1, connectionId2);

            //指定组的，除了调用者以外的
            //Clients.OthersInGroup(groupName);

            //指定userName
            //var userName = Context.User.Identity.Name;
            //Clients.User(userName);

            //指定userName列表的
            //Clients.Users(userNameList);
        }

        public string Send(string message)
        {
            return $"{DateTime.Now.ToString("hh:MM:ss")}:{message}";
        }

        public async Task<string> DoLongRuningThing(IProgress<int> progress)
        {
            for(int i = 0; i <= 100; i+= 5)
            {
                await Task.Delay(200);
                progress.Report(i);
            }
            return "Job complete!";
        }

        #region 阻塞等待
        public Task JoinGroup(string groupName)
        {
            return Groups.Add(Context.ConnectionId, groupName);
        }

        public Task LeaveGroup(string groupName)
        {
            return Groups.Remove(Context.ConnectionId, groupName);
        }
        #endregion

        #region 异步等待
        public async Task JoinGroupAsync(string groupName)
        {
            await Groups.Add(Context.ConnectionId, groupName);
        }

        public async Task LeaveGroupAsync(string groupName)
        {
            await Groups.Remove(Context.ConnectionId, groupName);
        }
        #endregion

        public override Task OnConnected()
        {
            return base.OnConnected();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stopCalled">如果故意断开而非超时导致，则为true,否则为false</param>
        /// <returns></returns>
        public override Task OnDisconnected(bool stopCalled)
        {
            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            return base.OnReconnected();
        }

        public void SendContextInfomation()
        {
            //connectionId
            var connectionId = Context.ConnectionId;

            //http header
            var header = Context.Request.Headers;

            //query string data
            var queryString = Context.Request.QueryString;
            var sameQueryString = Context.QueryString;
            var value = queryString["parameterName"];

            //cookies
            var cookies = Context.Request.Cookies;
            var sameCookies = Context.RequestCookies;

            //user infomation
            var user = Context.User;

            //http context,不要使用HttpContext.Current
            var httpContext = Context.Request.GetHttpContext();
            
        }


        public void PassStateWithClientAndServer()
        {
            var callerProxy = Clients.Caller;
            string userName = callerProxy.userName;
            string computerName = callerProxy.computerName;

            callerProxy.userName = "张三";
            callerProxy.computerName = "IOS";
            callerProxy.message = "from server";
        }

        public void GetHubException()
        {
            throw new HubException("Hub exception!", new { userName = Context.User.Identity.Name });
        }
    }
}