using IModels = LogBookReader.IModels;
using IFilters = LogBookReader.IFilters;
using Models = LogBookReader.Models;
using Filters = LogBookReader.Filters;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogBookReaderTest.Creators
{
    internal class CreateFilters
    {
        private readonly CreateModels _createModels;

        internal CreateFilters(CreateModels createModels)
        {
            _createModels = createModels;

            CreateAppCodes();
        }

        internal List<Filters.FilterAppCodes> AppCodes { get; set; } = new List<Filters.FilterAppCodes>();

        internal void CreateAppCodes()
        {
            foreach (Models.AppCodes itemModel in _createModels.AppCodes)
            {
                var itemFilter = new Filters.FilterAppCodes();
                itemFilter.Fill(itemModel);

                AppCodes.Add(itemFilter);
            }

            AssertCreateModels(_createModels.AppCodes,
                               AppCodes,
                               "Создание фильтров AppCodes");
        }

        private void AssertCreateModels<TExpected, TActual>(List<TExpected> listExpected,
                                                            List<TActual> listActual,
                                                            string message)
                                                                    where TExpected : IModels.IModelsBase
                                                                    where TActual : IFilters.IFilterBase
        {
            Assert.AreEqual(listExpected.Count, listActual.Count, message);

            for (int i = 0; i < listExpected.Count; i++)
            {
                Assert.AreEqual(listExpected[i].Code, listActual[i].Code, message);
                Assert.AreEqual(listExpected[i].Name, listActual[i].Name, message);
            }
        }

        private void CreatePrimitiveModels<TFilter, TModel>(List<TFilter> list, List<TModel> listModel)
                        where TFilter : LogBookReader.IFilters.IFilterBase, new()
                        where TModel : LogBookReader.IModels.IModelsBase
        {
            foreach (TModel itemModel in listModel)
            {
                TFilter itemFilter = new TFilter
                {
                    Code = itemModel.Code,
                    Name = itemModel.Name
                };

                list.Add(itemFilter);
            }
        }
    }
}
