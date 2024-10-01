const openModal = document.getElementById('chooseSubject');
const courseId = document.getElementById('courseId').value;


const RemoveDropDown = () => {
  const dropdown = document.getElementById('subjectsContainer');
  console.log(dropdown);
  dropdown.innerHTML = '';

  const div = document.createElement('div');
  div.id = 'subjects';
  dropdown.appendChild(div);
  console.log(dropdown);
}
const ShowSubjects = (data) =>  {
        let subjects = new ej.dropdowns.DropDownList({
        // Define the list items
        dataSource: data,
        // Map the appropriate columns to fields property
        fields: { text: 'name', value: 'id' } ,
        // Set the placeholder to DropDownList input element
        placeholder: 'Select a subject',
        // Set the height of the popup element
        popupHeight: '220px'
      });
      // Render initialized DropDownList component
      subjects.appendTo('#subjects');
};
const ShowError = (message) =>  {
  const div = document.getElementById('error');

  div.innerHTML = message;
  div.style.display = 'block';
  setTimeout(() => {
    div.style.display = 'none';
  }, 3000);
};
function endpointResult(result) {
  if(result.isSuccess) {
    ShowSubjects(result.data);
  } else {
    ShowError(result.message);
  }
}
openModal.addEventListener('click', async () => {

  const response = await fetch(`http://localhost:5286/Admin/Courses/GetSubjects?courseId=${courseId}`);

  if (response.ok){
       const result = await response.json();
       endpointResult(result);
  } else {
    ShowError('An error occurred, please try again');
  }
});

document.getElementById("cancel").addEventListener('click', () => {
  RemoveDropDown();
});
