/**
 * UI Modals
 */

'use strict';

document.addEventListener('DOMContentLoaded', function () {
  console.log('UI Modals');
   const openModal = document.querySelectorAll('.openModal');
    const deleteModal = document.getElementById('deleteUser');
    const modal = document.getElementById('basicModal');


   openModal.forEach((selectedUser) => {
      selectedUser.addEventListener('click', () => {
        deleteModal.dataset.userid = selectedUser.dataset.userid;
        deleteModal.dataset.userrole = selectedUser.dataset.userrole;

        console.log(`User with id: ${selectedUser.dataset.userid} and role: ${selectedUser.dataset.userrole} has been selected`, modal);
      });
   });

   deleteModal.addEventListener('click', (e) => {
      e.preventDefault();
     console.log('Delete User');
     deleteModal.textContent = "deleting...";
      deleteModal.disabled = true;
      const userId = deleteModal.dataset.userid;
      const userRole = deleteModal.dataset.userrole;
      console.log(`User with id: ${userId} and role: ${userRole} has been deleted`);

      setTimeout(() => {
        console.log(`User with id: ${userId} and role: ${userRole} has been deleted`);
        modal.classList.add('hide');

        window.location.href = `/Admin/Users/Delete/${userId}?role=${userRole}`;
      }, 500);
   });
});
