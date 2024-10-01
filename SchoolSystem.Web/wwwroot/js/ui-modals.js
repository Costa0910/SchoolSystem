/**
 * UI Modals
 */

'use strict';

document.addEventListener('DOMContentLoaded', function () {
  console.log('UI Modals');
   const openModal = document.querySelectorAll('.openModal');
    const deleteModal = document.getElementById('deleteModal');
    const modal = document.getElementById('basicModal');


   openModal.forEach((selectedUser) => {
      selectedUser.addEventListener('click', () => {
        deleteModal.dataset.id = selectedUser.dataset.id;
        deleteModal.dataset.userrole = selectedUser.dataset.userrole;

        console.log(`User with id: ${selectedUser.dataset.userid} and role: ${selectedUser.dataset.userrole} has been selected`, modal);
      });
   });

   deleteModal.addEventListener('click', (e) => {
      e.preventDefault();
     console.log('Delete User');
     deleteModal.textContent = "deleting...";
      deleteModal.disabled = true;
      const id = deleteModal.dataset.id;
      const userRole = deleteModal.dataset.userrole;
      const endpoint = deleteModal.dataset.endpoint;
      const sendTo = `${endpoint}/${id}?role=${userRole}`;
      console.log(`User with id: ${id} and role: ${userRole} has been deleted`);

      setTimeout(() => {
        console.log(`User with id: ${id} and role: ${userRole} has been deleted`);
        modal.classList.add('hide');
        window.location.href = sendTo;
      }, 500);
   });
});
