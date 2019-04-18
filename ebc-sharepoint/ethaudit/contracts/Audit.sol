pragma solidity >=0.4.21;

contract Audit {

    event DocumentStateChanged(uint documentId, string state);
    event DocumentAdded(uint documentId, address owner, string state);
    
    struct Document {
        uint DocumentId;
        address Owner;
        bytes32 DocumentHash;        
    }

    mapping(uint => string) public documentStates;    
    Document[] documents;
    uint documentCount;

    function addDocument(uint documentId, string memory state, bytes32 documentHash) public {
        uint id = documentCount++;

        Document memory doc = Document({
            DocumentId : id,
            Owner : msg.sender,
            DocumentHash : documentHash
        });

        documents.push(doc);
        documentStates[documentId] = state;        
        
        emit DocumentAdded(id, msg.sender, state);
    }

    function setState(uint documentId, string memory state, bytes32 documentHash) public {

        require(documents[documentId].DocumentHash == documentHash, "Document has changed.");

        documentStates[documentId] = state;
        
        emit DocumentStateChanged(documentId, state);
    }

    function getState(uint documentId) public view returns (string memory) {
        return documentStates[documentId];        
    }
    
    function getCount() public view returns (uint) {
        return documentCount;
    }
}
