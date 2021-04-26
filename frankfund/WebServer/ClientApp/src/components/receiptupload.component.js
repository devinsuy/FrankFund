import React, { useState } from 'react';

export default function Receipts({ receipts }) {
    return (
        <>
            { receipts.map((receipt) => (<Receipt key={receipt.TID} receipt={receipt} />))}
        </>
    )
}

function FileUploadPage() {

    const [selectedFile, setSelectedFile] = useState(); 
    const [isFilePicked, setIsFilePicked] = useState(false); 

    const changeHandler = (event) => {
        setSelectedFile(event.target.files[0]);
        setIsSelected(true);
    };

    const handleSubmission = () => {
    };

    return (
        <div>
            <input type="file" name="file" onChange={changeHandler} />
            {isSelected ? (
                <div>
                    <p>Filename: {selectedFile.name} </p>
                    <p>FileType: {selectedFile.type} </p>
                    <p>Size in bytes:{selectedFile.size}</p>
                    <p>
                        lastModifiedDate:{''}
                        {selectedFile.lastModifiedDate.toLocaleDateString()}
                    </p>
                </div>

            ) : (
                    <p> Select a file to show details </p>
                )}

            <div>
                <button onClick={handleSubmission}>Submit</button>
            </div>
        </div>
    )
}