import Swal from 'sweetalert2';
import $ from 'jquery';
import axios, { post } from 'axios';
import React, { Component } from 'react';

export default function ImageUploadDemo() {
    // Build api url for this particular goal
    const apikey = "c55f8d138f6ccfd43612b15c98706943e1f4bea3";
    const endpoint = `/api/receipt/upload&apikey=${apikey}`;


        // Prompt user to select a file to upload, input validated for images only
        async function promptUpload() {

            const { value: file } = await Swal.fire({
                title: 'Select image to upload',
                input: 'file',
                showCloseButton: true,
                showCancelButton: true,
                inputAttributes: {
                    'accept': 'image/*',
                    'aria-label': 'Upload your profile picture',
                    id: "fileToUpload"
                }
            })

            if (file) {
                const reader = new FileReader()
                reader.onload = async (e) => {
                    let fileInput = document.getElementById('fileToUpload');
                    let file = fileInput;
                    let formData = new FormData();
                    formData.append('imageFile', file);
                    
                    Swal.fire({
                        title: 'Your uploaded picture',
                        imageUrl: e.target.result,
                        imageAlt: 'The uploaded picture'
                    })
                }
                reader.readAsDataURL(file);
            }

            if (file) {
                const reader = new FileReader()
                reader.onload = (e) => {
                    const formData = new FormData;
                    formData.append('imageFile', file); //e.target.result[0]
                    console.log("here")
                    writeFile(formData);
                    Swal.fire({
                        title: 'Your uploaded picture',
                        imageUrl: e.target.result,
                        imageAlt: 'The uploaded picture'
                    })
                }
                Swal.fire("Hi");
            }


            else {
                Swal.fire("Nothing uploaded");
                return null;
            }

            //const { value: file } = await swal.fire({
            //  title: 'select image',
            //  input: 'file',
            //  inputattributes: {
            //    'accept': 'image/*',
            //    'aria-label': 'upload your profile picture'
            //  }
            //})

            if (file) {
                const reader = new FileReader()
                reader.onload = (e) => {
                    Swal.fire({
                        title: 'Your uploaded picture',
                        imageUrl: e.target.result,
                        imageAlt: 'The uploaded picture'
                    })
                }
                reader.readAsDataURL(file)
            }
        }

        // Make HTTP POST request to server to save the file into cloud storage
        async function writeFile() {
            console.log("Inside write file")
            let formData = new FormData();
            const h = {}; 
            //h.Accept = 'application/json';
           
            axios.post(endpoint, formData);

            // Build the request
            const params = {
                method: 'POST',
                headers: h,
                body: formData
            }

            const config = {
                headers: {
                    'content-type': 'multipart/form-data'
                }
            }


            // Send the request containing the image file to the api
            let imgUrl;
            let loading = true;
            while (loading) {
                // Show loading message
                Swal.fire({
                    title: `Uploading File`,
                    html: `<p>Uploading file to cloud storage</p>`,
                    allowOutsideClick: false,
                    onBeforeOpen: () => { Swal.showLoading() }
                });
                $.ajax({
                    method: "POST",
                    url: endpoint,
                    processData: false,
                    contentType: false,
                    data: formData
                }).done(function (data) { console.log(data); });

                await (
                    fetch(endpoint, params)
                        .then(response => response.json())
                        .then(data => {
                            const imgurl = endpoint;
                            // display success message
                            Swal.fire({
                                title: "image upload demo",
                                icon: "success",
                                html: `image was successfully uploaded to cloud storage bucket: ${imgurl}`,
                                showclosebutton: true
                            });
                        })
                        .catch(err => {
                            // display failed message
                            console.log(err);
                            Swal.fire({
                                title: "image upload demo",
                                icon: "error",
                                html: `<p>something went wrong, failed to upload image file!`,
                                showclosebutton: true
                            });
                        })
                );
                loading = false;
            }
            return post(imgUrl, formData, config)
        }

        return <>
            <div class="container">
                <button onClick={promptUpload} className="btn btn-outline-success btn-sm">Upload</button>

                <form method="post" action="uploadReceipt" enctype="multipart/form-data" id="myform">
                    <div >
                        <input type="file" id="file" name="file"/>
                        <input type="submit" class="button" value="Upload" id="but_upload" />
                    </div>
                </form>
            </div>
        </>

    }



