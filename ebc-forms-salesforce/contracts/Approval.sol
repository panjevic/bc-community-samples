pragma solidity >=0.5.0;

contract Approval {
    
    event ApprovalLogAdded(uint id, bytes32 opportunityHash, bytes32 actorIdHash, string date, string status);
    
    struct ApprovalLog {
        uint Id;        
        bytes32 OpportunityHash;
        bytes32 ActorIdHash;
        string Date;
        string Status;      
    }

    mapping(uint => ApprovalLog) public ApprovalLogs;

    uint LogCount;

    function addLog(bytes32 opportunityHash, bytes32 actorIdHash, string memory date, string memory status) public {
        uint id = LogCount++;

        ApprovalLog memory doc = ApprovalLog({
            Id : id,
            OpportunityHash : opportunityHash,
            ActorIdHash : actorIdHash,
            Date : date,
            Status : status
        });
        
        ApprovalLogs[id] = doc;                        

        emit ApprovalLogAdded(id, opportunityHash, actorIdHash, date, status);
    }

    function getCount() public view returns (uint) {
        return LogCount;
    }
}