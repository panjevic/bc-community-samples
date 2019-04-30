pragma solidity >=0.5.0;

contract AttestableDelivery {
    
    event DeliveryLogAdded(uint deliveryId, bytes32 photoHash, bytes32 detailsHash, bytes32 signatureHash, string date);
    
    struct DeliveryLog {
        uint DeliveryId;
        bytes32 PhotoHash;
        bytes32 DetailsHash;
        bytes32 SignatureHash;
        string Date;
    }

    mapping(uint => DeliveryLog) public DeliveryLogs;

    uint LogCount;

    function addLog(uint deliveryId, bytes32 photoHash, bytes32 detailsHash, bytes32 signatureHash, string memory date) public {
        uint id = LogCount++;

        DeliveryLog memory doc = DeliveryLog({
            DeliveryId : deliveryId,
            PhotoHash : photoHash,
            DetailsHash : detailsHash, 
            SignatureHash : signatureHash,
            Date : date            
        });
        
        DeliveryLogs[deliveryId] = doc;                        

        emit DeliveryLogAdded(deliveryId, photoHash, detailsHash, signatureHash, date);
    }

    function getCount() public view returns (uint) {
        return LogCount;
    }
}