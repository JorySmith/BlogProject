// Declare index and set to 0, references tag options[index] below
let index = 0;

// Add Swal mixin button
const swalWithDarkButton = Swal.mixin({
  customClass: {
    confirmButton: "btn btn-danger btn-sm btn-outline-dark",
    timer: 3000,
    buttonsStyling: false
  }
})

// Add post tags to select list
function addTag() {
  // store #TagEntry input
  let tagEntry = document.getElementById("TagEntry");

  // See if tag is empty or a duplicate already in TagList
  // If so, creat alert pop up
  let result = search(tagEntry.value);
  if (result != null) return swalWithDarkButton.fire({
    title: '<strong>Error</strong>',
    text: `${result}`,
    icon: 'info',
    confirmButtonText:
      '<i class="fa fa-thumbs-up"></i> Got it!',
    confirmButtonAriaLabel: 'Thumbs up, got it!'
  })

  // Create a new select option for each tag, add to #TagList
  let newTagOption = new Option(tagEntry.value, tagEntry.value);
  document.getElementById("TagList").options[index++] = newTagOption;

  // Clear out TagEntry input
  tagEntry.value = "";

  return true;
}

// Remove post tags from select list
function removeTag() {
  // track total tag counts 
  let tagCount = 1;
  let tagList = document.getElementById("TagList");
  // if no tag to remove, return false
  if (!tagList) return false;
  // if no tags selected (tagList.selectedIndex == -1), run swalWithDarkButton.fire
  if (tagList.selectedIndex == -1) return swalWithDarkButton.fire({
    title: '<strong>Error</strong>',
    text: 'Please select a tag to remove.',
    icon: 'info',
    confirmButtonText:
      '<i class="fa fa-thumbs-up"></i> Got it!',
    confirmButtonAriaLabel: 'Thumbs up, got it!'
  })

  // while tagCount > 0, set selected tag option to null
  // if tagList.selectedIndex >= 0, set tagList.options[tagList.selectedIndex] = null;
  // decrement tagCount
  // otherwiise, set tagCount to 0 and decrement index
  while (tagCount > 0) {
    if (tagList.selectedIndex >= 0) {
      tagList.options[tagList.selectedIndex] = null;
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

// Set initial tags, if tagValues has tags, split tagValues into array on ","
// Loop over array, create replaceTag function, increment index
if (tagValues != "") {
  let tagArray = tagValues.split(",");

  for (let idx = 0; idx < tagArray.length; idx++) {
    // Add or replace tags accordingly
    replaceTag(tagArray[idx], idx);
    index++;
  }
}

function replaceTag(tag, index) {
  let newOption = new Option(tag, tag);
  document.getElementById("TagList").options[index] = newOption;
}

// See if tag is empty or a duplicate already in TagList
// If error, return an error string
function search(str) {
  if (str == "") return "Please provide a non-empty tag.";

  // Store current tags in TagList
  let tagsEl = document.getElementById("TagList");

  // If there are tags, store select options 
  // 
  if (tagsEl) {
    let options = tagsEl.options;
    for (let i = 0; i < options.length; i++) {
      if (options[i].value == str) return `Tag "${str}" is a duplicate tag. Please provide a unique tag.`
    }
  }

}
