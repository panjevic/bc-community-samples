
const CrypticLegendsToken = artifacts.require("CrypticLegendsToken");
const BigNumber = web3.BigNumber;

const should = require('chai')
  .use(require('chai-bignumber')(BigNumber))
  .should();


contract('CrypticLegendsToken tests', function(accounts) {

  let crypticLegendsTokenInstance;

  beforeEach(async () => {
    crypticLegendsTokenInstance = await CrypticLegendsToken.deployed();
  });

/*  it("should be an ERC721", async () => {
    const InterfaceId_ERC721 = 0x80ac58cd;
    assert.isTrue(await crypticLegendsTokenInstance.supportsInterface(InterfaceId_ERC721 ));
  });
*/
  it("should be an ERC721 contract called CrypticLegendsToken", async () => {
    assert.equal("CrypticLegendsToken", await crypticLegendsTokenInstance.name());
  });

  
  it("Should make first account an owner", async () => {
    let owner = await crypticLegendsTokenInstance.owner();
    assert.equal(owner, accounts[0]);
  });

  describe("Cryptic Legends Character tests", () => {
    const firstTokenId = 1;
    const secondTokenId = 2;
    const thirdTokenId = 3;
    const fourthTokenId = 4;
    const minter = accounts[0];
    const receiver = accounts[1];

    it("mints to 0 address", async () => {
      try {
        await crypticLegendsTokenInstance.mint(address(0), firstTokenId);
      } catch (error) {
        error.message.should.include('address is not defined', 'Wrong failure type');
        return;
      }
      should.fail('Expected `address is not defined` failure not received');
    });

    it("creates two characters", async () => {
      await crypticLegendsTokenInstance.mint(receiver, firstTokenId);
      await crypticLegendsTokenInstance.mint(receiver, secondTokenId);
      let totalSupply = await crypticLegendsTokenInstance.totalSupply();
      totalSupply.should.be.bignumber.equal(2);
    });
    it("creates third character and validates the owner by ID", async () => {
      await crypticLegendsTokenInstance.mint(receiver, thirdTokenId);
      let thirdTokenOwner = await crypticLegendsTokenInstance.ownerOf(thirdTokenId);
      thirdTokenOwner.should.be.equal(receiver);
    });

    it("checks if only minter can mint tokens", async () => {
      try {
        await crypticLegendsTokenInstance.mint(minter, thirdTokenId, {from: receiver});
      } catch (error) {
        error.message.should.include('revert', 'Wrong failure type');
        return;
      }
      should.fail('Expected `revert` failure not received');
    });

    it("creates character with attributes", async () => {
      const immutableAttr = 5;
      const mutableAttr = 3;
      await crypticLegendsTokenInstance.mintCharacter(
                    receiver,
                    fourthTokenId, 
                    immutableAttr,
                    mutableAttr,
                    {from: minter});
      let fourthToken = await crypticLegendsTokenInstance.getToken(fourthTokenId);
      
      assert.equal(fourthToken[0], immutableAttr);
      assert.equal(fourthToken[1], mutableAttr);
    });

    
    it("checks if character with immutable attribute can be out of balance", async () => {
      const immutableAttr = 7;
      const mutableAttr = 3;
      const nextTokenId = await crypticLegendsTokenInstance.totalSupply();
      try {
        await crypticLegendsTokenInstance.mintCharacter(
                      receiver, 
                      nextTokenId+1, 
                      immutableAttr,
                      mutableAttr,
                      {from: minter});
      } catch (error) {
        error.message.should.include('revert', 'Wrong failure type');
        return;
      }
      
      should.fail('Expected `revert` failure not received');
    });

    
    it("checks if character with immutable attribute can be abouve the allowed range", async () => {
      const immutableAttr = 15;
      const mutableAttr = 3;
      const nextTokenId = await crypticLegendsTokenInstance.totalSupply();
      try {
        await crypticLegendsTokenInstance.mintCharacter(
                      receiver, 
                      nextTokenId+1, 
                      immutableAttr,
                      mutableAttr,
                      {from: minter});
      } catch (error) {
        error.message.should.include('revert', 'Wrong failure type');
        return;
      }
      
      should.fail('Expected `revert` failure not received');
    });

    
    it("checks if character with immutable attribute can be abouve the allowed range", async () => {
      const immutableAttr = 2;
      const mutableAttr = 3;
      const nextTokenId = await crypticLegendsTokenInstance.totalSupply();
      try {
        await crypticLegendsTokenInstance.mintCharacter(
                      receiver, 
                      nextTokenId+1, 
                      immutableAttr,
                      mutableAttr,
                      {from: minter});
      } catch (error) {
        error.message.should.include('revert', 'Wrong failure type');
        return;
      }
      
      should.fail('Expected `revert` failure not received');
    });

    it("checks if character mutable attribute can be above the allowed range", async () => {
      const immutableAttr = 5;
      const mutableAttr = 13;
      const nextTokenId = await crypticLegendsTokenInstance.totalSupply();
      try {
        await crypticLegendsTokenInstance.mintCharacter(
                      receiver, 
                      nextTokenId+1, 
                      immutableAttr,
                      mutableAttr,
                      {from: minter});
      } catch (error) {
        error.message.should.include('revert', 'Wrong failure type');
        return;
      }
      
      should.fail('Expected `revert` failure not received');
    });

    it("checks if character mutable attribute can be below the allowed range", async () => {
      const immutableAttr = 5;
      const mutableAttr = 2;
      const nextTokenId = await crypticLegendsTokenInstance.totalSupply();
      try {
        await crypticLegendsTokenInstance.mintCharacter(
                      receiver, 
                      nextTokenId+1, 
                      immutableAttr,
                      mutableAttr,
                      {from: minter});
      } catch (error) {
        error.message.should.include('revert', 'Wrong failure type');
        return;
      }
      
      should.fail('Expected `revert` failure not received');
    });

    it("checks if character default attributes are correct", async () => {
      const immutableAttrDefault = 5;
      const mutableAttrDefault = 3;
      const nextTokenId = await crypticLegendsTokenInstance.totalSupply();
      await crypticLegendsTokenInstance.mint(
                    receiver, 
                    nextTokenId+1,
                    {from: minter});
      let token = await crypticLegendsTokenInstance.getToken(nextTokenId+1);
      
      assert.equal(token[0].toNumber(), immutableAttrDefault);
      assert.equal(token[1].toNumber(), mutableAttrDefault);
    });


    it("checks if character mutable attribute can be leveled up", async () => {
      const immutableAttr = 5;
      const mutableAttr = 8;
      let nextTokenId = await crypticLegendsTokenInstance.totalSupply();
      nextTokenId++;
      await crypticLegendsTokenInstance.mintCharacter(
                    receiver, 
                    nextTokenId, 
                    immutableAttr,
                    mutableAttr,
                    {from: minter});
      await crypticLegendsTokenInstance.levelUpCharacter(nextTokenId);
      let token = await crypticLegendsTokenInstance.getToken(nextTokenId);
      
      assert.equal(token[1].toNumber(), mutableAttr+1);
    });

    
    it("checks if character mutable fails to go beyond the level limit", async () => {
      const immutableAttr = 5;
      const mutableAttr = 12;
      let nextTokenId = await crypticLegendsTokenInstance.totalSupply();
      nextTokenId++;
      await crypticLegendsTokenInstance.mintCharacter(
                    receiver, 
                    nextTokenId, 
                    immutableAttr,
                    mutableAttr,
                    {from: minter});
      try {
        await crypticLegendsTokenInstance.levelUpCharacter(nextTokenId);
      } catch (error) {
        error.message.should.include('revert', 'Wrong failure type');
        return;
      }
      
      should.fail('Expected `revert` failure not received');
    });

  });

});
