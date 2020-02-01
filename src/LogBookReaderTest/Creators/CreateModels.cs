using Models = LogBookReader.Models;
using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogBookReaderTest.Creators
{
    internal class CreateModels
    {
        private readonly Random _random = new Random((int)DateTime.Now.Ticks);

        internal CreateModels()
        {
            CreateAppCodes();
        }

        internal List<Models.AppCodes> AppCodes { get; set; } = new List<Models.AppCodes>();


        internal void CreateAppCodes()
        {
            int count = _random.Next(5, 15);

            CreatePrimitiveModels(AppCodes, count);

            AssertCreateModels(AppCodes,
                               count,
                               "Создание моделей AppCodes");
        }

        private void AssertCreateModels<T>(List<T> list,
                                           int count,
                                           string message) where T : LogBookReader.IModels.IModelsBase
        {
            Assert.AreEqual(count, list.Count, message);

            int i = 0;
            foreach (T item in list)
            {
                Assert.AreEqual("Name " + i++, item.Name, message);
            }

        }

        private void CreatePrimitiveModels<T>(List<T> list, int count) where T : LogBookReader.IModels.IModelsBase, new()
        {
            for (int i = 0; i < count; i++)
            {
                T newItem = new T()
                {
                    Code = i,
                    Name = "Name " + i
                };

                list.Add(newItem);
            }
        }

    }
}
