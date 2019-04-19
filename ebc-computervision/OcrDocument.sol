pragma solidity >=0.5.0;

// // This simple contract is used to showcase interaction with Ethereum network and should not be used as a reference point for any smart contract implementations
contract OcrDocument {
    
    event DocumentAdded(uint documentId, address owner, string fileId, string name, string path, string tag, string text, bytes32 documentHash);
    
    struct Document {
        uint Id;
        address Owner;  
        string FileId;        
        string Name;
        string Path;
        string Tag;
        string Text;
        bytes32 DocumentHash;
    }

    mapping(uint => Document) public documents;    
    
    uint documentCount;

    function addDocument(string memory fileId, string memory name, string memory path, string memory tag, string memory text, bytes32 documentHash) public {
        uint id = documentCount++;

        Document memory doc = Document({
            Id : id,
            Owner : msg.sender,            
            FileId : fileId,
            Name : name,
            Path : path,
            Tag : tag,
            Text : text,
            DocumentHash : documentHash
        });
        
        documents[id] = doc;        
        
        emit DocumentAdded(id, msg.sender, fileId, name, path, tag, text, documentHash);
    }

    function getText(uint documentId) public view returns (string memory) {
        return documents[documentId].Text;        
    }
    
    function getCount() public view returns (uint) {
        return documentCount;
    }
}