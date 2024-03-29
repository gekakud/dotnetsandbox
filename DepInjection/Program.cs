﻿using System;
using Autofac;
using DepInjection.DataProvider;

namespace DepInjection
{
    internal class Program
    {
        //bootstrapper with Autofac container
        private static void Main()
        {
            //Dependency injection (Simple way)
            var resolver = new RandomResolver();
            var d1 = new RetreiveData(resolver.ResolveProvider());
            var d2 = new RetreiveData(resolver.ResolveProvider());

            //Dependency injection with IOC container
            IContainer iocContainer = new BootStrapper().Bootstrap();

            //InTextIndexer will be created while its ctor will receive appropriate(registered) ICacheService instance
            InTextIndexer textIndexer = iocContainer.Resolve<InTextIndexer>();
            textIndexer.WhichCacheDoIuse();

            Console.ReadKey();
        }
    }

    internal class BootStrapper
    {
        public IContainer Bootstrap()
        {
            var builder = new ContainerBuilder();

            //register all dependencies(types)
            builder.RegisterType<RedisCache>().As<ICacheService>();
            builder.RegisterType<InTextIndexer>().As<InTextIndexer>();

            return builder.Build();
        }
    }

    //this class is going to use Cache service functionality
    //cache service is passed into ctor
    internal class InTextIndexer
    {
        private ICacheService _cacheService;

        //constructor injection
        public InTextIndexer(ICacheService p_cacheService)
        {
            //now we can use _cacheService and we dont care which one
            //of implementation is passed. all we know is interface contract
            _cacheService = p_cacheService;
        }

        public void WhichCacheDoIuse()
        {
            Console.WriteLine("\nusing " + _cacheService.GetType().Name);
        }
    }

    public interface ICacheService
    {
        bool SetString(string p_ent);
        string GetStringValueById(int p_id);
    }

    //two different implementations of ICacheService
    internal class RedisCache : ICacheService
    {
        public bool SetString(string p_ent)
        {
            return true;
        }

        public string GetStringValueById(int p_id)
        {
            return GetType().Name;
        }
    }

    internal class AppFabricCache : ICacheService
    {
        public bool SetString(string p_ent)
        {
            return true;
        }

        public string GetStringValueById(int p_id)
        {
            return GetType().Name;
        }
    }
}