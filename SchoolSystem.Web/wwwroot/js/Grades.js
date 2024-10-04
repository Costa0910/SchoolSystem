              // each subject with its grades and the edit button
              // <li class="mb-4">
              //   <div class="d-flex align-items-center">
              //     <div class="d-flex align-items-center">
              //       <div class="me-2">
              //         <h6 class="mb-0">Cecilia Payne</h6>
              //         <small>45 Connections</small>
              //       </div>
              //     </div>
              //     <div class="ms-auto">
              //       <i class="ri-edit-box-line ri-22px btn-outline-primary"
              //          data-bs-target="#modals-transparent" data-bs-toggle="modal">
              //       </i>
              //     </div>
              //   </div>
              // </li>

console.log("Grades.js");
  const btnListSubject = document.querySelectorAll(".ListStudentSubjects");
  const subjectUl = document.getElementById("StudentGrades");
  const modalTitle = document.getElementById("studentName");


    btnListSubject.forEach((btn) => {
      btn.addEventListener("click", async () => {
        const courseId = btn.dataset.course;
        const studentName = btn.dataset.author;
        const studentId = btn.dataset.student;

        modalTitle.innerHTML = `Grades of ${studentName}`;

        subjectUl.innerHTML = "";
        const li = document.createElement("li");
        li.innerHTML = `
          <div class="d-flex align-items-center justify-content-center">
          Loading ${studentName} grades...
          </div>
        `;
        subjectUl.appendChild(li);

        await GetStudentGrades(courseId, studentId);
      });
  });
// Result

// [
//     {
//         "id": "b642bb3e-3cd6-410a-4437-08dce0615572",
//         "name": "Quimica",
//         "grade": 0
//     },
//     {
//         "id": "12bfb0a3-0a7d-41a5-2c67-08dce06c1fe6",
//         "name": "Portugues",
//         "grade": 0
//     },
//     {
//         "id": "440cbabd-8243-4049-2c68-08dce06c1fe6",
//         "name": "Geografia",
//         "grade": 0
//     },
//     {
//         "id": "c4c3a630-bcf7-45e9-2c69-08dce06c1fe6",
//         "name": "Web client",
//         "grade": 0
//     },
//     {
//         "id": "64809585-352d-42f7-2c6a-08dce06c1fe6",
//         "name": "web server",
//         "grade": 0
//     }
// ]

async function GetStudentGrades(courseId, studentId) {
 const response = await fetch(`/Staff/Grades/GetStudentGrades?courseId=${courseId}&studentId=${studentId}`)
debugger;
 if (response.ok) {
   const data = await response.json();
   subjectUl.innerHTML = "";
   data.forEach((subject) => {
     const li = document.createElement("li");
     li.className = "mb-4";
     li.innerHTML = `
       <div class="d-flex align-items-center">
         <div class="d-flex align-items-center">
           <div class="me-2">
             <h6 class="mb-0">${subject.name}</h6>
             <small>${subject.grade} Grade</small>
           </div>
         </div>
         <div class="ms-auto">
           <i class="ri-edit-box-line ri-22px btn-outline-primary"
              data-bs-target="#modals-transparent" data-bs-toggle="modal"
              data-student=${studentId} data-course=${courseId} data-subject=${subject.id}>
           </i>
         </div>
       </div>
     `;
     subjectUl.appendChild(li);
   });
 } else {
   subjectUl.innerHTML = "";
   const li = document.createElement("li");
   li.innerHTML = `
     <div class="d-flex align-items-center justify-content-center">
     Error loading grades
     </div>
   `;
   subjectUl.appendChild(li);
 }
}
