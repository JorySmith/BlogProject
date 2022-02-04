// declare index and set to 0, references tag options[index] below
let index = 0;

// Add post tags to select list
function addTag() {
    // store #TagEntry input
    let tagEntry = document.getElementById("TagEntry");

    // create a new select option for tag, add to #TagList
    let newTagOption = new Option(tagEntry.value, tagEntry.value);
    document.getElementById("TagList").options[index++] = newTagOption;

    // clear out TagEntry input
    tagEntry.value = "";

    return true;
}

// Remove post tags from select list
function removeTag() {
    // track total tag counts with tagCount
    let tagCount = 1;

    // while tagCount > 0, get TagList, store selectedIndex
    // if selectedIndex >= 0, set tagList.options[selectedIndex] = null; 
    // decrement tagCount
    // otherwiise, set tagCount to 0 and decrement index
    while (tagCount > 0) {
        let tagList = document.getElementById("TagList");
        let selectedIndex = tagList.selectedIndex;
        if (selectedIndex >= 0) {
            tagList.options[selectedIndex] = null;
            tagCount--;
            index--;
        } else {
            tagCount = 0;
            
        }
    }
}

// Using jQuery, select all tags on Posts Create View form submit so they are posted to DB
$("form").on("submit", function () {
    $("#TagList option").prop("selected", "selected");
}) 