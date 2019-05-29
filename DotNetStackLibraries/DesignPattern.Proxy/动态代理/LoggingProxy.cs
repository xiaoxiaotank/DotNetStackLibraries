using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace DesignPattern.Proxy
{
    class LoggingProxy<TSubject> : DispatchProxy
    {
        private TSubject _subject;
        private Action<string> _logger;

        public static TSubject Create(TSubject subject, Action<string> logger)
        {
            //创建用于代理T类型、基于DynaProxy<T>的代理类
            object proxy = Create<TSubject, LoggingProxy<TSubject>>();
            ((LoggingProxy<TSubject>)proxy).SetParameters(subject, logger);

            return (TSubject)proxy;
        }

        private void SetParameters(TSubject subject, Action<string> logger)
        {
            _subject = subject;
            _logger = logger;
        }

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            _logger(targetMethod.Name);
            var result = targetMethod.Invoke(_subject, args);
            return result;
        }
    }
}
