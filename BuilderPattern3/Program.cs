using System;
using System.Collections.Generic;

namespace BuilderPattern3
{
    class Program
    {
        private static AppServiceFacade facade;

        static void Main(string[] args)
        {
            facade = BuildAppFacade();
        }

        static AppServiceFacade BuildAppFacade()
        {
            var facade = AppServiceFacadeBuilder
                .AppDalFacade()
                .WithTabId(new Guid())
                .WithDataProvider(p_in => p_in + string.Empty)
                .WithMessageService(new DummyUiService())
                .WithDeserializer(DummyParserService.Deserialize)
                .Build();

            return facade;
        }
    }

    public interface IUiService
    {
    }

    class DummyUiService : IUiService
    {
    }

    static class DummyParserService
    {
        public static Func<string, string> Deserialize { get; set; }
    }

    public class AppServiceFacade
    {
        public IUiService UiMessageService { get; set; }
        public Func<string, string> Deserializer { get; set; }
        public Func<string, string> RetriveEntitiesFromWebService { get; set; }
        public Func<string> RetriveNotificationsFromWebService { get; set; }
        public Guid TabId { get; set; }

        #region Constructor

        public AppServiceFacade()
        {
        }

        public AppServiceFacade(AppServiceFacade p_partial)
        {
            UiMessageService = p_partial.UiMessageService;
            Deserializer = p_partial.Deserializer;
            RetriveEntitiesFromWebService = p_partial.RetriveEntitiesFromWebService;
            RetriveNotificationsFromWebService = p_partial.RetriveNotificationsFromWebService;
            TabId = p_partial.TabId;
        }

        #endregion
    }

    public class AppServiceFacadeBuilder : ITabIdHolder, IDataLoaderHolder,
        IDalServiceFacadeBuilder, IMessageBoxPresenter, IDeserializer
    {
        #region Constructor

        private AppServiceFacade _partialFacade;

        private AppServiceFacadeBuilder()
        {
        }

        public static ITabIdHolder AppDalFacade()
        {
            return new AppServiceFacadeBuilder();
        }

        #endregion

        public IDataLoaderHolder WithTabId(Guid p_id)
        {
            _partialFacade = new AppServiceFacade {TabId = p_id};
            return this;
        }

        public IMessageBoxPresenter WithDataProvider(Func<string, string> p_dataProvider)
        {
            _partialFacade = new AppServiceFacade(_partialFacade)
            {
                RetriveEntitiesFromWebService = p_dataProvider
            };
            return this;
        }

        public IDeserializer WithMessageService(IUiService p_showMessageAction)
        {
            _partialFacade = new AppServiceFacade(_partialFacade)
            {
                UiMessageService = p_showMessageAction
            };
            return this;
        }

        public IDalServiceFacadeBuilder WithDeserializer(Func<string, string> p_deserializerFunc)
        {
            _partialFacade = new AppServiceFacade(_partialFacade)
            {
                Deserializer = p_deserializerFunc
            };
            return this;
        }

        public AppServiceFacade Build()
        {
            return _partialFacade;
        }
    }

    #region Builder Interfaces

    public interface ITabIdHolder
    {
        IDataLoaderHolder WithTabId(Guid p_id);
    }

    public interface IDataLoaderHolder
    {
        IMessageBoxPresenter WithDataProvider(Func<string, string> p_dataProvider);
    }

    public interface IMessageBoxPresenter
    {
        IDeserializer WithMessageService(IUiService p_showMessageAction);
    }

    public interface IDeserializer
    {
        IDalServiceFacadeBuilder WithDeserializer(Func<string, string> p_deserializerFunc);
    }

    public interface IDalServiceFacadeBuilder
    {
        AppServiceFacade Build();
    }

    #endregion
}
