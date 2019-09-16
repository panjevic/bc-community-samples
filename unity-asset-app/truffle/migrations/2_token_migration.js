var Token = artifacts.require("./CrypticLegendsToken.sol");

module.exports = function(deployer) {
  deployer.deploy(Token);
};