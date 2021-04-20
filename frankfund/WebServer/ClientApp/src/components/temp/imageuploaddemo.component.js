import Swal from 'sweetalert2';
import $ from 'jquery';

export default function ImageUploadDemo(){
    // Build api url for this particular goal
    const apikey = "c55f8d138f6ccfd43612b15c98706943e1f4bea3";
    const endpoint = `/api/receipt/upload&apikey=${apikey}`;

    // Prompt user to select a file to upload, input validated for images only
    // async function promptUpload(){
    //     const { value: file} = await Swal.fire({
    //         title: 'Select image to upload',
    //         input: 'file',
    //         showCloseButton: true,
    //         showCancelButton: true,
    //         inputAttributes: {
    //           'accept': 'image/*',
    //           'aria-label': 'Upload your profile picture',
    //           id: "fileToUpload"
    //         }
    //       })

    //       if (file) {
    //         const reader = new FileReader()
    //         reader.onload = async (e) => {
    //             let fileInput = document.getElementById('fileToUpload');
    //             let file = fileInput.files[0];
    //             let formData = new FormData();
    //             formData.append('ImageFile', file);
    //             await writeFile(formData);
    //             Swal.fire({
    //                 title: 'Your uploaded picture',
    //                 imageUrl: e.target.result,
    //                 imageAlt: 'The uploaded picture'
    //             })
    //         }
    //         reader.readAsDataURL(file)
    //       }
          
    //     //   if (file) {
    //     //     const reader = new FileReader()
    //     //     reader.onload = (e) => {
    //             // const formData = new FormData;
    //             // formData.append('ImageFile', e.target.result);
    //             // console.log("here")
    //     //         // writeFile(formData);      
    //     //       Swal.fire({
    //     //         title: 'Your uploaded picture',
    //     //         imageUrl: e.target.result,
    //     //         imageAlt: 'The uploaded picture'
    //     //       })
    //     //     }   
    //     //  }
    //     //     Swal.fire("Hi");
          
    //       else{
    //           Swal.fire("Nothing uploaded");
    //           return null;
    //       }

    //     //   const { value: file } = await Swal.fire({
    //     //     title: 'Select image',
    //     //     input: 'file',
    //     //     inputAttributes: {
    //     //       'accept': 'image/*',
    //     //       'aria-label': 'Upload your profile picture'
    //     //     }
    //     //   })
          
    //     //   if (file) {
    //     //     const reader = new FileReader()
    //     //     reader.onload = (e) => {
    //         //   Swal.fire({
    //         //     title: 'Your uploaded picture',
    //         //     imageUrl: e.target.result,
    //         //     imageAlt: 'The uploaded picture'
    //         //   })
    //         // }
    //     //     reader.readAsDataURL(file)
    //     //   }
    // }

    // // Make HTTP POST request to server to save the file into cloud storage
    // async function writeFile(formData){
    //     console.log("Inside write file")
    //     // Build the request
    //     const params = {
    //         method: 'POST',
    //         body: formData       
    //     }   

    //     // Send the request containing the image file to the api
    //     let imgUrl;
    //     let loading = true; 
    //     while(loading){
    //         // Show loading message
    //         Swal.fire({
    //             title: `Uploading File`,
    //             html: `<p>Uploading file to cloud storage</p>`,
    //             allowOutsideClick: false,
    //             onBeforeOpen: () => { Swal.showLoading() }
    //         });
    //         $.ajax({
    //             method: "POST",
    //             url: endpoint,
    //             processData: false,
    //             contentType: false,
    //             data: formData
    //         }).done(function (data) { console.log(data); });

    //         // await(
    //         //     fetch(endpoint, params)
    //         //         .then(response => response.json())
    //         //         .then(data => { 
    //         //             imgUrl = data.url;
    //         //             // Display success message
    //         //             Swal.fire({
    //         //                 title: "Image Upload Demo",
    //         //                 icon: "success",
    //         //                 html: `Image was successfully uploaded to cloud storage bucket: ${imgUrl}`,
    //         //                 showCloseButton: true
    //         //             });
    //         //         })
    //         //         .catch(err => { 
    //         //             // Display failed message
    //         //             console.log(err); 
    //         //             Swal.fire({
    //         //                 title: "Image Upload Demo",
    //         //                 icon: "error",
    //         //                 html: `<p>Something went wrong, failed to upload image file!`,
    //         //                 showCloseButton: true
    //         //             });
    //         //         })
    //         // );
    //         loading = false;
    //     }

    // }


    return <>
        <div class="container">
            <form method="post" action={endpoint} enctype="multipart/form-data" id="myform">
                <div >
                    <input type="file" id="file" name="file" />
                    <input type="submit" class="button" value="Upload" id="but_upload" />
                </div>
            </form>
        </div>
    </>

}


