// Path: SchoolSystem.Web/wwwroot/js/Enrollment.js
// Filter students
// Delete Student from Course
let debounceTimeout;

function filterStudents(e) {
  clearTimeout(debounceTimeout);
  debounceTimeout = setTimeout(() => {
    const filter = e.target.value.toUpperCase();
    console.log('Filtering Students by:', filter);
    const students = document.querySelectorAll('.elements');
    students.forEach((subject) => {
      const name = subject.querySelector('.element-name').textContent;
      if (name.toUpperCase().includes(filter)) {
        subject.style.display = '';
      } else {
        subject.style.display = 'none';
      }
    });
  }, 500); // 300ms delay
}
document.addEventListener('DOMContentLoaded', function () {
  console.log('Input Filter');
  // addListener to filter input
   const filterInput = document.getElementById('filterInput');
   filterInput.addEventListener("keyup", filterStudents);

   // Modal
  console.log('UI Modals');
   const openModal = document.querySelectorAll('.openModal');
    const deleteModal = document.getElementById('deleteModal');
    const modal = document.getElementById('basicModal');




   openModal.forEach((selected) => {
      selected.addEventListener('click', () => {
        deleteModal.dataset.element = selected.dataset.element;
        deleteModal.dataset.course = selected.dataset.course;
        deleteModal.dataset.endpoint = selected.dataset.endpoint;
        console.log(`element with id: ${selected.dataset.element} and course: ${selected.dataset.course} has been selected`, modal);
      });
   });

   deleteModal.addEventListener('click', (e) => {
      e.preventDefault();
     console.log("Deleting student from course");
     deleteModal.textContent = "deleting...";
      deleteModal.disabled = true;
      const element = deleteModal.dataset.element;
      const courseId = deleteModal.dataset.course;
      const elementEndpoint = deleteModal.dataset.endpoint;
      // get base url
      const splitUrl = window.location.href.split('/');
      const baseUrl = splitUrl[0] + '//' + splitUrl[2];
      const endpoint = `${baseUrl}${elementEndpoint}`;
      const sendTo = `${endpoint}?id=${element}&courseId=${courseId}`;

      setTimeout(() => {
      console.log((`Course with id: ${courseId} and Student: ${element} has been deleted`), sendTo);
        modal.classList.add('hide');
        window.location.href = sendTo;
      }, 500);
   });
});
