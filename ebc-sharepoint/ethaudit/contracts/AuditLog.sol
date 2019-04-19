pragma solidity >=0.5.1;

// This simple contract is used to showcase interaction with Ethereum network and should not be used as a reference point for any smart contract implementations
contract AuditLog {

    event LogAdded(uint id, address owner, string email);
    
    
    struct LogTrace {
        uint Id;
        uint ItemId;
        address Owner;
        string ModifiedDate;
        string ModifiedBy;
        string ModifiedByName;
    }

    mapping(uint => LogTrace) public logs;
    uint logCount;

    function addLog(uint itemId, string memory date, string memory email, string memory name) public {
        logCount++;

        LogTrace memory log = LogTrace({
            Id : logCount,
            ItemId : itemId,
            Owner : msg.sender,
            ModifiedDate : date,
            ModifiedBy : email,
            ModifiedByName : name
        });
        
        logs[logCount] = log;        
        
        emit LogAdded(logCount, msg.sender, email);
    }
    
    function getCount() public view returns (uint) {
        return logCount;
    }
}
