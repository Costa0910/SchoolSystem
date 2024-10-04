// Path: SchoolSystem.Web/wwwroot/js/Enrollment.js
// Filter students
// Delete Student from Course
let debounceTimeout;

function filterStudents(e) {
  clearTimeout(debounceTimeout);
  debounceTimeout = setTimeout(() => {
    const filter = e.target.value.toUpperCase();
    console.log('Filtering Students by:', filter);
    const students = document.querySelectorAll('.students');
    students.forEach((subject) => {
      const name = subject.querySelector('.student-name').textContent;
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




   openModal.forEach((selectedUser) => {
      selectedUser.addEventListener('click', () => {
        deleteModal.dataset.student = selectedUser.dataset.student;
        deleteModal.dataset.course = selectedUser.dataset.course;
        console.log(`User with id: ${selectedUser.dataset.student} and role: ${selectedUser.dataset.course} has been selected`, modal);
      });
   });

   deleteModal.addEventListener('click', (e) => {
      e.preventDefault();
     console.log("Deleting student from course");
     deleteModal.textContent = "deleting...";
      deleteModal.disabled = true;
      const studentId = deleteModal.dataset.student;
      const courseId = deleteModal.dataset.course;
      // get base url
      const splitUrl = window.location.href.split('/');
      const baseUrl = splitUrl[0] + '//' + splitUrl[2];
      const endpoint = `${baseUrl}/Staff/Enrollments/DeleteStudent`;
      const sendTo = `${endpoint}?studentId=${studentId}&courseId=${courseId}`;

      setTimeout(() => {
      console.log((`Course with id: ${courseId} and Student: ${studentId} has been deleted`), sendTo);
        modal.classList.add('hide');
        window.location.href = sendTo;
      }, 500);
   });
});
