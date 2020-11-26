using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using UsedProductExchange.Core.Domain;
using UsedProductExchange.Core.Entities;

namespace UsedProductExchange.XUnitTestProject
{
    class CategoryServiceTest
    {
        private List<Category> _categories = null;
        private readonly Mock<IRepository<Category>> _repoMock;

        public CategoryServiceTest()
        {
            _repoMock = new Mock<IRepository<Category>>();
            _repoMock.Setup(repo => repo.GetAll()).Returns(() => _categories);
        }
    }
}
