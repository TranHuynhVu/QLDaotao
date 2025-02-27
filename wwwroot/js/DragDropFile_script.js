const fileInput = document.querySelector(".file-selector-input-dragdrop");
const previewContainer = document.querySelector(".list-dragdrop");
let selectedFiles = [];
const dropArea = document.querySelector(".drop-section-dragdrop");
const fileSelector = document.querySelector(".file-selector-dragdrop");

fileSelector.onclick = (e) => {
    e.preventDefault();
    fileInput.click();
    console.log(document.querySelector(".file-selector-dragdrop"));
};

// Khi chọn file từ input
fileInput.onchange = () => {
    [...fileInput.files].forEach((file) => {
        if (typeValidation(file.type)) {
            addFile(file);
        }
    });
};

// Khi file được kéo vào drop zone
dropArea.ondragover = (e) => {
    e.preventDefault();
    dropArea.classList.add("drag-over-effect");
};

dropArea.ondragleave = () => {
    dropArea.classList.remove("drag-over-effect");
};

dropArea.ondrop = (e) => {
    e.preventDefault();
    dropArea.classList.remove("drag-over-effect");

    [...e.dataTransfer.files].forEach((file) => {
        if (typeValidation(file.type)) {
            addFile(file);
        }
    });
};

function addFile(file) {
    if (!selectedFiles.some((f) => f.name === file.name)) {
        selectedFiles.push(file);
        renderPreview();
    } else {
        alert("File đã tồn tại trong danh sách!");
    }
}

function renderPreview() {
    $(previewContainer).empty(); // Xóa danh sách trước khi render lại

    if (selectedFiles.length === 0) {
        $(".list-section-dragdrop").hide(); // Ẩn danh sách nếu không có file
        $(previewContainer).html("<p>Không có tệp nào được chọn</p>");
        return;
    } else {
        $(".list-section-dragdrop").show(); // Hiển thị danh sách nếu có file
    }

    selectedFiles.forEach((file, index) => {
        if (!file.type.startsWith("image/")) return;

        var reader = new FileReader();
        reader.onload = function (e) {
            var card = $(`
                <li class="complete w-100">
                   <div class="col d-flex justify-content-between align-items-center">
                         <svg xmlns="http://www.w3.org/2000/svg" width="30px" fill="#5874c6" viewBox="0 0 384 512">
                             <path d="M64 0C28.7 0 0 28.7 0 64L0 448c0 35.3 28.7 64 64 64l256 0c35.3 0 64-28.7 64-64l0-288-128 0c-17.7 0-32-14.3-32-32L224 0 64 0zM256 0l0 128 128 0L256 0zM64 256a32 32 0 1 1 64 0 32 32 0 1 1 -64 0zm152 32c5.3 0 10.2 2.6 13.2 6.9l88 128c3.4 4.9 3.7 11.3 1 16.5s-8.2 8.6-14.2 8.6l-88 0-40 0-48 0-48 0c-5.8 0-11.1-3.1-13.9-8.1s-2.8-11.2 .2-16.1l48-80c2.9-4.8 8.1-7.8 13.7-7.8s10.8 2.9 13.7 7.8l12.8 21.4 48.3-70.2c3-4.3 7.9-6.9 13.2-6.9z"/>
                         </svg>
                     </div>
                     <div class="col w-50">
                         <div class="file-name">
                             <div class="name">${truncateFileName(file.name)}</div>
                         </div>
                         <div class="file-size">${(file.size / (1024 * 1024)).toFixed(2)} MB</div>
                     </div>
                     <div class="col d-flex justify-content-between align-items-center">
                         <svg xmlns="http://www.w3.org/2000/svg" class="cancel-upload" onClick="removeFile('${file.name}')" width="20px" fill="#dde4f6" viewBox="0 0 512 512">
                             <path d="M256 512A256 256 0 1 0 256 0a256 256 0 1 0 0 512zM175 175c9.4-9.4 24.6-9.4 33.9 0l47 47 47-47c9.4-9.4 24.6-9.4 33.9 0s9.4 24.6 0 33.9l-47 47 47 47c9.4 9.4 9.4 24.6 0 33.9s-24.6 9.4-33.9 0l-47-47-47 47c-9.4 9.4-24.6 9.4-33.9 0s-9.4-24.6 0-33.9l47-47-47-47c-9.4-9.4-9.4-24.6 0-33.9z"/>
                         </svg>
                     </div>
                </li>
            `);

            $(previewContainer).append(card);
        };

        reader.readAsDataURL(file);
    });

    updateFileInput();
}

function truncateFileName(name, maxLength = 30) {
    if (name.length > maxLength) {
        return name.substring(0, maxLength - 5) + "....";
    }
    return name;
}

function removeFile(fileName) {
    selectedFiles = selectedFiles.filter((file) => file.name !== fileName);
    updateFileInput();
    renderPreview();
}

function updateFileInput() {
    var dataTransfer = new DataTransfer();
    selectedFiles.forEach((file) => dataTransfer.items.add(file));
    fileInput.files = dataTransfer.files;

    if (selectedFiles.length === 0) {
        fileInput.value = "";
    }
}

function typeValidation(fileType) {
    return fileType.startsWith("image/");
}
