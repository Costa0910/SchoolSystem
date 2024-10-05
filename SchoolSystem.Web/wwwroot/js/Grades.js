// Load student grades
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

async function GetStudentGrades(courseId, studentId) {
 const response = await fetch(`/Staff/Grades/GetStudentGrades?courseId=${courseId}&studentId=${studentId}`)

 if (response.ok) {
   const data = await response.json();
   subjectUl.innerHTML = "";
   data.forEach((subject) => {
     const li = document.createElement("li");
     li.className = "mb-4";
     let noun = subject.grade < 2 ? "value" : "values";
     let gradeColor = subject.grade < 10 ? "text-danger" : "text-primary";
     let gradeString = subject.grade == 0 ? "Not graded yet" : `${subject.grade} ${noun}`;
     li.innerHTML = `
       <div class="d-flex align-items-center">
         <div class="d-flex align-items-center">
           <div class="me-2">
             <h6 class="mb-0">${subject.name}</h6>
              <small class="${gradeColor}" id=${subject.id}>${gradeString}</small>
           </div>
         </div>
         <div class="ms-auto">
           <i class="ri-edit-box-line ri-22px btn-outline-primary openGradeModal"
              data-bs-target="#modals-transparent" data-bs-toggle="modal"
              data-student=${studentId} data-course=${courseId} data-subject=${subject.id} data-value=${subject.grade}>
           </i>
         </div>
       </div>
     `;
     subjectUl.appendChild(li);
   });

    const btnListSubject = document.querySelectorAll(".openGradeModal");
    btnListSubject.forEach((btn) => {
      btn.addEventListener("click", async (e) => {

        console.log("openGradeModal click", btn);
        e.preventDefault();
        e.stopPropagation();

        const courseId = btn.dataset.course;
        const studentId = btn.dataset.student;
        const subjectId = btn.dataset.subject;
        const value = btn.dataset.value

        await GetSelectedSubjectInfo(courseId, studentId, subjectId, value);
      });
    });
 } else {
   subjectUl.innerHTML = "";
   const li = document.createElement("li");
   li.innerHTML = `
     <div class="d-flex align-items-center justify-content-center text-danger">
     Error loading grades
     </div>
   `;
   subjectUl.appendChild(li);
 }
}


console.log("Grades.js - grades modal");

async function GetSelectedSubjectInfo(courseId, studentId, subjectId, value) {
  const inputGrade = document.getElementById("gradeValue");
  inputGrade.value = value == 0 ? "" : value; // Set the input value

  const btnSave = document.getElementById("btnSaveGrade");
  const divError = document.getElementById("gradeError");
  const close = document.getElementById("btnCloseGrade");

  // Clear any existing event listeners(avoid multiple event listeners)
  btnSave.onclick = async (e) => {
    e.preventDefault();
    e.stopPropagation();

    const grade = inputGrade.value;

    // close the modal if the old value is the same as the new value
    if (grade == value) {
      close.click();
      return;
    }

    if (CheckValidGrade(grade)) {
      await SaveGrade(courseId, studentId, subjectId, grade);
    } else {
      divError.style.display = "block";
      divError.classList.add("text-danger");
      divError.innerHTML = "Invalid grade value";
      setTimeout(() => {
        divError.style.display = "none";
      }, 3000);
    }
  };
}

const CheckValidGrade = (grade) =>  {
  const parsedGrade = parseFloat(grade);

  if (isNaN(parsedGrade)) {
    return false;
  } else if (parsedGrade <= 0 || parsedGrade > 20)
    return false;

  return true;
}
async function SaveGrade(courseId, studentId, subjectId, grade) {
    const btnSave = document.getElementById("btnSaveGrade");
    btnSave.disabled = true;
    btnSave.innerHTML = "Saving...";

    const body = {
      courseId: courseId,
      studentId: studentId,
      subjectId: subjectId,
      grade: grade
    };


    const response = await fetch(`/Staff/Grades/SaveGrade`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify(body)
    });


    if (response.ok) {
      const gradeElement = document.getElementById(subjectId);
      const close = document.getElementById("btnCloseGrade");
      let noun = grade < 2 ? "value" : "values";
      let gradeColor = grade < 10 ? "text-danger" : "text-primary";
      let gradeString = grade == 0 ? "Not graded yet" : `${grade} ${noun}`;
      gradeElement.innerHTML = gradeString;
      gradeElement.className = gradeColor;
      btnSave.disabled = false;
      btnSave.innerHTML = "Save";
      close.click();
    } else {
      btnSave.disabled = false;
      btnSave.innerHTML = "Save";
      const divError = document.getElementById("gradeError");
      divError.innerHTML = "Error saving grade";
      divError.classList.add("text-danger");
      divError.style.display = "block";
      setTimeout(() => {
        divError.style.display = "none";
        }, 3000);
    }
}
