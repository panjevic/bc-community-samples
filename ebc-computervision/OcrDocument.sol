pragma solidity >=0.5.0;

contract OcrDocument {
    
    event DocumentAdded(uint documentId, address owner, string documentUri, string textUri);
    
    struct Document {
        uint DocumentId;
        address Owner;  
        string DocumentUri;
        string TextUri;
        bytes32 DocumentHash;
    }

    mapping(uint => Document) public documents;    
    
    uint documentCount;

    function addDocument(uint documentId, string memory documentUri, string memory textUri, bytes32 documentHash) public {
        documentCount++;

        Document memory doc = Document({
            DocumentId : documentId,
            Owner : msg.sender,
            DocumentUri : documentUri,
            TextUri : textUri,
            DocumentHash : documentHash
        });
        
        documents[documentId] = doc;        
        
        emit DocumentAdded(documentId, msg.sender, documentUri, textUri);
    }

    function getTextUri(uint documentId) public view returns (string memory) {
        return documents[documentId].TextUri;        
    }

    function getDocumentUri(uint documentId) public view returns (string memory) {
        return documents[documentId].DocumentUri;        
    }
    
    function getCount() public view returns (uint) {
        return documentCount;
    }
}