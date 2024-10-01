/**
 * UI Modals
 */

'use strict';

let debounceTimeout;

function filterSubjects(e) {
  clearTimeout(debounceTimeout);
  debounceTimeout = setTimeout(() => {
    const filter = e.target.value.toUpperCase();
    console.log('Filtering Subjects by:', filter);
    const subjects = document.querySelectorAll('.subject');
    subjects.forEach((subject) => {
      const subjectName = subject.querySelector('.subject-name').textContent;
      if (subjectName.toUpperCase().includes(filter)) {
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
   filterInput.addEventListener("keyup", filterSubjects);

   // Modal
  console.log('UI Modals');
   const openModal = document.querySelectorAll('.openModal');
    const deleteModal = document.getElementById('deleteModal');
    const modal = document.getElementById('basicModal');




   openModal.forEach((selectedUser) => {
      selectedUser.addEventListener('click', () => {
        deleteModal.dataset.subject = selectedUser.dataset.subject;
        deleteModal.dataset.course = selectedUser.dataset.course;
        console.log(`User with id: ${selectedUser.dataset.subject} and role: ${selectedUser.dataset.course} has been selected`, modal);
      });
   });

   deleteModal.addEventListener('click', (e) => {
      e.preventDefault();
     console.log('Delete subject');
     deleteModal.textContent = "deleting...";
      deleteModal.disabled = true;
      const subjectId = deleteModal.dataset.subject;
      const courseId = deleteModal.dataset.course;
      // get base url
      const splitUrl = window.location.href.split('/');
      const baseUrl = splitUrl[0] + '//' + splitUrl[2];
      const endpoint = `${baseUrl}/Admin/Courses/DeleteSubjectFromCourse`;
      const sendTo = `${endpoint}?subjectId=${subjectId}&courseId=${courseId}`;

      setTimeout(() => {
      console.log((`Course with id: ${courseId} and Subject: ${subjectId} has been deleted`), sendTo);
        modal.classList.add('hide');
        window.location.href = sendTo;
      }, 500);
   });
});
