using TokenService.Exception;
using TokenService.Model.Entity;
using TokenService.Repository;
using Xunit;
using Xunit.Abstractions;

namespace TokenServiceTest.Repository
{
    public class TokenInMemRepositoryTest
    {
        private readonly ITestOutputHelper output;

        public TokenInMemRepositoryTest(ITestOutputHelper output)
        {
            this.output = output;
        }


        [Fact]
        public void CreateNullEntity()
        {
            TokenInMemRepository ut = new TokenInMemRepository();
            try
            {
                ut.Create(null);
                Assert.False(true);
            }
            catch (BadArgumentException e)
            {
                output.WriteLine("Received expected BadArgumentException: " + e.Message);
            }
        }

        [Fact]
        public void CreateNullKey()
        {
            // create no key
            TokenEntity myEntity = new TokenEntity(null, null);
            TokenInMemRepository ut = new TokenInMemRepository();
            try
            {
                ut.Create(myEntity);
                Assert.False(true);
            }
            catch (BadArgumentException e)
            {
                output.WriteLine("Received expected BadArgumentException: " + e.Message);
            }
        }

        [Fact]
        public void GetNullId()
        {
            TokenInMemRepository ut = new TokenInMemRepository();
            try
            {
                ut.GetById(null);
                Assert.False(true, "should have failed GetById(null) with exception :-(");
            }
            catch (BadArgumentException e)
            {
                output.WriteLine("Received expected exception when retrieving with null Id : " + e.Message);
            }
        }

        [Fact]
        public void CreateGet()
        {
            TokenIdentityEntity obo = new TokenIdentityEntity("myIdentityProvider", "myUsername");
            TokenIdentityEntity initiator = new TokenIdentityEntity("myIdentityProvider", "myUsername");
            TokenEntity myEntity = CreateValidTokenEntity(obo, initiator);
            TokenInMemRepository ut = new TokenInMemRepository();
            try
            {
                ut.Create(myEntity);
                TokenEntity retrieved = ut.GetById("my-unique-identifier");
                Assert.NotNull(retrieved);
                // should test equality
            }
            catch (BadArgumentException e)
            {
                Assert.False(true, "Received unexpected BadArgumentException: " + e.Message);
            }
        }

        [Fact]
        public void DeleteNull()
        {
            TokenInMemRepository ut = new TokenInMemRepository();
            try
            {
                ut.Delete(null);
                output.WriteLine("Correctly ignored null object during delete request");
            }
            catch (BadArgumentException e)
            {
                Assert.False(true, "Should have silently eaten Delete(null) " + e.Message);
            }
        }

        [Fact]
        public void DeleteNullKey()
        {
            TokenEntity myEntity = CreateValidTokenEntity(null, null);
            TokenInMemRepository ut = new TokenInMemRepository();
            myEntity.JwtUniqueIdentifier = null;
            try
            {
                ut.Delete(myEntity);
                Assert.False(true, "Should have failed to delete object that does not have primary key value");
            }
            catch (BadArgumentException e)
            {
                output.WriteLine("Correctly failed deleting object missing primary key: " + e.Message);
            }
        }

        [Fact]
        public void CreateGetDeleteGet()
        {
            TokenIdentityEntity obo = new TokenIdentityEntity("myIdentityProvider", "myUsername");
            TokenIdentityEntity initiator = new TokenIdentityEntity("myIdentityProvider", "myUsername");
            TokenEntity myEntity = CreateValidTokenEntity(obo, initiator);
            TokenInMemRepository ut = new TokenInMemRepository();
            try
            {
                ut.Create(myEntity);
                TokenEntity retrieved = ut.GetById("my-unique-identifier");
                Assert.NotNull(retrieved);
                ut.Delete(myEntity);
                Assert.Null(ut.GetById("my-unique-identifier"));
            }
            catch (BadArgumentException e)
            {
                Assert.False(true, "Received unexpected BadArgumentException: " + e.Message);
            }
        }

        [Fact]
        public void CreateDeleteDeleteGet()
        {
            TokenIdentityEntity obo = new TokenIdentityEntity("myIdentityProvider", "myUsername");
            TokenIdentityEntity initiator = new TokenIdentityEntity("myIdentityProvider", "myUsername");
            TokenEntity myEntity = CreateValidTokenEntity(obo, initiator);
            TokenInMemRepository ut = new TokenInMemRepository();
            try
            {
                ut.Create(myEntity);
                // another test verifies it was actually stored
                ut.Delete(myEntity);
                ut.Delete(myEntity);
                Assert.Null(ut.GetById("my-unique-identifier"));
            }
            catch (BadArgumentException e)
            {
                Assert.False(true, "Received unexpected BadArgumentException: " + e.Message);
            }
        }

        [Fact]
        public void UpdateNullEntity()
        {
            TokenInMemRepository ut = new TokenInMemRepository();
            try
            {
                ut.Update(null);
                Assert.False(true);
            }
            catch (BadArgumentException e)
            {
                output.WriteLine("Received expected BadArgumentException: " + e.Message);
            }
        }

        [Fact]
        public void UpdateNullKey()
        {
            // create no key
            TokenEntity myEntity = new TokenEntity(null, null);
            TokenInMemRepository ut = new TokenInMemRepository();
            try
            {
                ut.Update(myEntity);
                Assert.False(true);
            }
            catch (BadArgumentException e)
            {
                output.WriteLine("Received expected BadArgumentException: " + e.Message);
            }
        }

        [Fact]
        public void CreateUpdateGet()
        {
            TokenIdentityEntity obo1 = new TokenIdentityEntity("myIdentityProvider", "myUsername1");
            TokenIdentityEntity obo2 = new TokenIdentityEntity("myIdentityProvider", "myUsername2");
            TokenIdentityEntity initiator1 = new TokenIdentityEntity("myIdentityProvider", "myUsername1");
            TokenEntity myEntity1 = CreateValidTokenEntity(obo1, initiator1);
            TokenEntity myEntity2 = CreateValidTokenEntity(obo2, initiator1);
            TokenInMemRepository ut = new TokenInMemRepository();
            try
            {
                ut.Create(myEntity1);
                Assert.Same(myEntity1, ut.GetById(myEntity1.Id));
                ut.Update(myEntity2);
                Assert.Same(myEntity2, ut.GetById(myEntity1.Id));
            }
            catch (BadArgumentException e)
            {
                Assert.False(true, "Received unexpected BadArgumentException: " + e.Message);
            }
        }

        [Fact]
        public void UpdateUpdateGet()
        {
            TokenIdentityEntity obo1 = new TokenIdentityEntity("myIdentityProvider", "myUsername1");
            TokenIdentityEntity obo2 = new TokenIdentityEntity("myIdentityProvider", "myUsername2");
            TokenIdentityEntity initiator1 = new TokenIdentityEntity("myIdentityProvider", "myUsername1");
            TokenEntity myEntity1 = CreateValidTokenEntity(obo1, initiator1);
            TokenEntity myEntity2 = CreateValidTokenEntity(obo2, initiator1);
            TokenInMemRepository ut = new TokenInMemRepository();
            try
            {
                ut.Update(myEntity1);
                Assert.Same(myEntity1, ut.GetById(myEntity1.Id));
                ut.Update(myEntity2);
                Assert.Same(myEntity2, ut.GetById(myEntity1.Id));
            }
            catch (BadArgumentException e)
            {
                Assert.False(true, "Received unexpected BadArgumentException: " + e.Message);
            }
        }

#pragma warning disable CA1822
        private TokenEntity CreateValidTokenEntity(TokenIdentityEntity obo, TokenIdentityEntity initiator)
#pragma warning restore CA1822
        {
            return new TokenEntity(obo, initiator)
            {
                Version = "1.0",
                ProtectedUrl = "http://www.foo.com",
                JwtUniqueIdentifier = "my-unique-identifier",
            };
        }
    }
}
