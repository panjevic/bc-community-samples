pragma solidity >=0.5.0;

contract CrypticLegendsToken {

  address private _owner;

  uint8 private constant levelDefault = 5;
  uint8 private constant levelMax = 20;
  //median value is used to restrict the median value of abilities given. Also this is the default value
  uint8 private constant levelMedian = 12;

  uint8 private tokenCount = 0;


  // Mapping from token ID to owner
  mapping (uint256 => address) private _tokenOwner;
  // Mapping from owner to number of owned token
  mapping (address => uint256) private _ownedTokensCount;

  /**
    Name
    String containing the hero's name.

    Core Attributes
    Core attributes can have an integer value from the interval [5, 20].

    Wisdom (WIS)
    Bonus to mental defense and mental effect potency.

    Intelligence (INT)
    Bonus to certain mental actions’ success.

    Charisma (CHA)
    Bonus to certain mental actions’ success.

    Speed (SPD)
    Lowers the duration of performing actions.


    Accuracy (ACC)
    Bonus to physical actions’ success.

    Might (MGT)
    Determines hero’s maximum health and gives bonus to physical effect potency.
   */

  //Main Character object definition
  struct Character {
    string name;
    uint8 wisdom;
    uint8 inteligence;
    uint8 charisma;
    uint8 speed;
    uint8 accuracy;
    uint8 might;
  }


  // Optional mapping for token URIs
  mapping(uint256 => string) private _tokenURIs;

  mapping(uint256 => Character) public tokenProperty;



  //EVENTS
  event OwnershipTransferred(address indexed previousOwner, address indexed newOwner);
  event Transfer(address from, address to, uint256 tokenId);
  

  


  /**
    * @dev The Ownable constructor sets the original `owner` of the contract to the sender
    * account.
    */
  constructor () public {
      _owner = msg.sender;
  }



  /**
    * @dev Returns whether the specified token exists.
    * @param tokenId uint256 ID of the token to query the existence of
    * @return bool whether the token exists
    */
  function _exists(uint256 tokenId) internal view returns (bool) {
      address _tmpOwner = _tokenOwner[tokenId];
      return _tmpOwner != address(0);
  }

  /**
    * @dev Returns whether the given spender can transfer a given token ID.
    * @param spender address of the spender to query
    * @param tokenId uint256 ID of the token to be transferred
    * @return bool whether the msg.sender is approved for the given token ID,
    * is an operator of the owner, or is the owner of the token
    */
  function _isApprovedOrOwner(address spender, uint256 tokenId) internal view returns (bool) {
      require(_exists(tokenId), "ERC721: operator query for nonexistent token");
      address _tmpOwner = ownerOf(tokenId);
      return (spender == _tmpOwner);
  }

  /**
    * @return the address of the owner.
    */
  function owner() public view returns (address) {
      return _owner;
  }

  /**
    * @return true if `msg.sender` is the owner of the contract.
    */
  function isOwner() public view returns (bool) {
      return msg.sender == _owner;
  }
  
  
  //MODIFIERS
  /**
    * @dev Throws if called by any account other than the owner.
    */
  modifier onlyOwner() {
      require(isOwner(), "Ownable: caller is not the owner");
      _;
  }


  /**
    * @dev Allows the current owner to relinquish control of the contract.
    * It will not be possible to call the functions with the `onlyOwner`
    * modifier anymore.
    * @notice Renouncing ownership will leave the contract without an owner,
    * thereby removing any functionality that is only available to the owner.
    */
  function renounceOwnership() public onlyOwner {
      emit OwnershipTransferred(_owner, address(0));
      _owner = address(0);
  }

  /**
    * @dev Allows the current owner to transfer control of the contract to a newOwner.
    * @param newOwner The address to transfer ownership to.
    */
  function transferOwnership(address newOwner) public onlyOwner {
      _transferOwnership(newOwner);
  }

  /**
    * @dev Transfers control of the contract to a newOwner.
    * @param newOwner The address to transfer ownership to.
    */
  function _transferOwnership(address newOwner) internal {
      require(newOwner != address(0), "Ownable: new owner is the zero address");
      emit OwnershipTransferred(_owner, newOwner);
      _owner = newOwner;
  }

  /** Ownable end */


  /**
    * @dev Returns an URI for a given token ID.
    * Throws if the token ID does not exist. May return an empty string.
    * @param tokenId uint256 ID of the token to query
    */
  function tokenURI(uint256 tokenId) external view returns (string memory) {
      require(_exists(tokenId), "ERC721Metadata: URI query for nonexistent token");
      return _tokenURIs[tokenId];
  }

  /**
    * @dev Internal function to set the token URI for a given token.
    * Reverts if the token ID does not exist.
    * @param tokenId uint256 ID of the token to set its URI
    * @param uri string URI to assign
    */
  function _setTokenURI(uint256 tokenId, string memory uri) internal {
      require(_exists(tokenId), "ERC721Metadata: URI set of nonexistent token");
      _tokenURIs[tokenId] = uri;
  }



  /**
    * @dev Gets the owner of the specified token ID.
    * @param tokenId uint256 ID of the token to query the owner of
    * @return address currently marked as the owner of the given token ID
    */
  function ownerOf(uint256 tokenId) public view returns (address) {
      address _tmpOwner = _tokenOwner[tokenId];
      require(_tmpOwner != address(0), "ERC721: owner query for nonexistent token");

      return _tmpOwner;
  }


    /**
    * @dev Gets the balance of the specified address.
    * @param _tmpOwner address to query the balance of
    * @return uint256 representing the amount owned by the passed address
    */
  function balanceOf(address _tmpOwner) public view returns (uint256) {
      require(_tmpOwner != address(0), "ERC721: balance query for the zero address");

      return _ownedTokensCount[_tmpOwner];
  }

  modifier levelInRange(uint _level) {
    require(_level <= levelMax && _level >= levelDefault , 'Level not in range');
    _;
  }


  /**
    * @dev Internal function to mint a new token.
    * Reverts if the given token ID already exists.
    * @param to The address that will own the minted token
    * @param tokenId uint256 ID of the token to be minted
    */
  function _mint(address to, uint256 tokenId) internal {
      require(to != address(0), "ERC721: mint to the zero address");
      require(!_exists(tokenId), "ERC721: token already minted");

      _tokenOwner[tokenId] = to;
      _ownedTokensCount[to] += 1;

      emit Transfer(address(0), to, tokenId);
  }


  function mintCharacter(address _to, string memory _name, uint8 _wisdom, uint8 _inteligence
                        , uint8 _charisma, uint8 _speed, uint8 _accuracy, uint8 _might) 
              public
              onlyOwner
              returns(bool) {
              
    tokenCount += 1;
    tokenProperty[tokenCount] = Character({
      name: _name,
      wisdom: _wisdom,
      inteligence: _inteligence,
      charisma: _charisma,
      speed: _speed,
      accuracy: _accuracy,
      might: _might
    });

    _mint(_to, tokenCount);
    return true; 
  }



  function mint(address _to, string memory _name) public onlyOwner returns(bool) {
    return mintCharacter(_to, _name, levelDefault, levelDefault, levelDefault, levelDefault, levelDefault, levelDefault);
  }

  function getToken(uint _tokenId) public view returns(string memory, uint8, uint8, uint8, uint8, uint8, uint8) {
    return (tokenProperty[_tokenId].name
          , tokenProperty[_tokenId].wisdom
          , tokenProperty[_tokenId].inteligence
          , tokenProperty[_tokenId].charisma
          , tokenProperty[_tokenId].speed
          , tokenProperty[_tokenId].accuracy
          , tokenProperty[_tokenId].might
          );
  }
  
  /**
    * @dev Returns total amount of toknes minted 
    *
   */
  function getTokenCount() public view returns(uint8) {
    return tokenCount;
  }
  

  /**
    * @dev Internal function to transfer ownership of a given token ID to another address.
    * As opposed to transferFrom, this imposes no restrictions on msg.sender.
    * @param from current owner of the token
    * @param to address to receive the ownership of the given token ID
    * @param tokenId uint256 ID of the token to be transferred
    */
  function _transferFrom(address from, address to, uint256 tokenId) internal {
      require(ownerOf(tokenId) == from, "transfer of token that is not own");
      require(to != address(0), "transfer to the zero address");

      _ownedTokensCount[from] -= 1;
      _ownedTokensCount[to] += 1;

      _tokenOwner[tokenId] = to;

      emit Transfer(from, to, tokenId);
  }



  /**
    * @dev Transfers the ownership of a given token ID to another address.
    * Usage of this method is discouraged, use `safeTransferFrom` whenever possible.
    * Requires the msg.sender to be the owner, approved, or operator.
    * @param from current owner of the token
    * @param to address to receive the ownership of the given token ID
    * @param tokenId uint256 ID of the token to be transferred
    */
  function transferFrom(address from, address to, uint256 tokenId) public {
      //solhint-disable-next-line max-line-length
      require(_isApprovedOrOwner(msg.sender, tokenId), "ERC721: transfer caller is not owner");

      _transferFrom(from, to, tokenId);
  }

  /**
    * @dev Safely transfers the ownership of a given token ID to another address
    * If the target address is a contract, it must implement `onERC721Received`,
    * which is called upon a safe transfer, and return the magic value
    * `bytes4(keccak256("onERC721Received(address,address,uint256,bytes)"))`; otherwise,
    * the transfer is reverted.
    * Requires the msg.sender to be the owner, approved, or operator
    * @param from current owner of the token
    * @param to address to receive the ownership of the given token ID
    * @param tokenId uint256 ID of the token to be transferred
    */
  function safeTransferFrom(address from, address to, uint256 tokenId) public {
      transferFrom(from, to, tokenId);
  }



  /**
    * @dev Internal function to burn a specific token.
    * Reverts if the token does not exist.
    * Deprecated, use _burn(uint256) instead.
    * @param _tmpOwner owner of the token to burn
    * @param tokenId uint256 ID of the token being burned
    */
  function _burn(address _tmpOwner, uint256 tokenId) internal {
      require(ownerOf(tokenId) == _tmpOwner, "burn of token that is not own");

      _ownedTokensCount[_tmpOwner] -= 1;
      _tokenOwner[tokenId] = address(0);

      emit Transfer(_tmpOwner, address(0), tokenId);
  }

  /**
    * @dev Internal function to burn a specific token.
    * Reverts if the token does not exist.
    * @param tokenId uint256 ID of the token being burned
    */
  function _burn(uint256 tokenId) internal {
      _burn(ownerOf(tokenId), tokenId);
  }
}
