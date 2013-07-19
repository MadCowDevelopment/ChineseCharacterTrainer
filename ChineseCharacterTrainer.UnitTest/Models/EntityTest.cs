using System;
using ChineseCharacterTrainer.Model;
using NUnit.Framework;

namespace ChineseCharacterTrainer.UnitTest.Models
{
    public class EntityTest
    {
        [Test]
        public void EntitiesWithSameIdShouldBeEqual()
        {
            var guid = Guid.NewGuid();
            var entity1 = new TestEntity(guid);
            var entity2 = new TestEntity(guid);

            Assert.IsTrue(entity1.Equals(entity2));
        }

        [Test]
        public void EntitiesWithDifferentIdShouldNotBeEqual()
        {
            var entity1 = new TestEntity(Guid.NewGuid());
            var entity2 = new TestEntity(Guid.NewGuid());

            Assert.IsFalse(entity1.Equals(entity2));
        }

        [Test]
        public void EntitiesWhereOneIsNullShouldNotBeEqual()
        {
            var entity = new TestEntity(new Guid());

            Assert.IsFalse(entity.Equals(null));
        }

        [Test]
        public void EntitiesWithDifferentTypeShouldNotBeEqual()
        {
            var entity = new TestEntity(new Guid());

            Assert.IsFalse(entity.Equals(new object()));
        }

        private class TestEntity : Entity
        {
            public TestEntity(Guid id)
            {
                Id = id;
            }
        }
    }
}
