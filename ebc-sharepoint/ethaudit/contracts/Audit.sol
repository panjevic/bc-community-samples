pragma solidity >=0.5.1;

contract Audit {

    event DocumentStateChanged(uint documentId, string state);
    event DocumentAdded(uint documentId, address owner, string state);
    
    struct Document {
        uint DocumentId;
        address Owner;
        string State;
        bytes32 DocumentHash;        
    }

    mapping(uint => Document) public documents;    
    
    uint documentCount;

    function addDocument(uint documentId, string memory state, bytes32 documentHash) public {
        documentCount++;

        Document memory doc = Document({
            DocumentId : documentId,
            Owner : msg.sender,
            State : state,
            DocumentHash : documentHash
        });
        
        documents[documentId] = doc;        
        
        emit DocumentAdded(documentId, msg.sender, state);
    }

    function setState(uint documentId, string memory state, bytes32 documentHash) public {

        require(documents[documentId].DocumentHash == documentHash, "Document has changed.");

        documents[documentId].State = state;
        
        emit DocumentStateChanged(documentId, state);
    }

    function getState(uint documentId) public view returns (string memory) {
        return documents[documentId].State;        
    }
    
    function getCount() public view returns (uint) {
        return documentCount;
    }
}
