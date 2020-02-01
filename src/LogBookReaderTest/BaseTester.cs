using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogBookReaderTest
{
    [TestClass]
    public class BaseTester
    {
        internal Creators.CreateFilters _createFilters;
        internal Creators.CreateModels _createModels;

        [TestMethod]
        public void Creators()
        {
            _createModels = new Creators.CreateModels();
            _createFilters = new Creators.CreateFilters(_createModels);


        }
    }
}
