console.log("showGrade.js loaded");


//global variables
const caption = document.getElementById("caption");
const tableBody = document.getElementById("tableBody");
const tableFooter = document.getElementById("tableFooter");
document.addEventListener("DOMContentLoaded", function () {

    // Get the button
    let studentBtn = document.querySelectorAll(".showGradeButton");

    // Add event listener to the button
    studentBtn.forEach(async function (btn) {
      btn.addEventListener("click", async function () {
              tableBody.innerHTML = "";
              tableFooter.innerHTML = "";
              const courseId = btn.dataset.course;
              const studentId = btn.dataset.student;
              const studentName = btn.dataset.author;

              caption.innerHTML = `Grades for ${studentName}`;
              const row = document.createElement("tr");
              row.innerHTML = `
              <td colspan="4">Loading...</td>
              `;
              tableBody.appendChild(row);
              await GetAllGradesOfStudent(courseId, studentId);
      });
    });

  async function GetAllGradesOfStudent(courseId, studentId) {

    const response = await fetch(`GetStudentGrades/?courseId=${courseId}&studentId=${studentId}`);

    debugger;
    if (response.ok) {
      const data = await response.json();
      if (data.length > 0) {
        ShowGrades(data);
      } else {
        tableBody.innerHTML = "";
        const row = document.createElement("tr");
        row.innerHTML = `
        <td colspan="4">No grades found for this student</td>
        `;
        tableBody.appendChild(row);
      }
    } else {
      console.error("Failed to fetch data");
      alert("Failed to fetch student grades, please try again later");
    }
  }

  const ShowGrades = (data) =>  {
    tableBody.innerHTML = "";
    tableFooter.innerHTML = "";
    let total = 0.0;
    data.forEach((gradeRaw, i) => {
      const grade = parseGrade(gradeRaw);
      const row = document.createElement("tr");
      const status = grade.status === "Pass" ? "text-success" : "text-danger";
      row.innerHTML = `
      <td scope="row">${i + 1}</td>
      <td>${grade.name}</td>
      <td>${grade.value}</td>
      <td class=${status}>${grade.status}</td>
      `;
      tableBody.appendChild(row);
      let value = parseFloat(grade.value);
      if (!isNaN(value)) {
        total += value;
      }
    });

    const row = document.createElement("tr");
    const average = total / data.length;
    const status = average >= 10 ? "text-success" : "text-danger";
    let roundedAverage = Math.round(average * 100) / 100;

    if (total <= 0) {
      total = "Not graded yet";
      roundedAverage = "Not graded yet";
    }

    row.innerHTML = `
    <td>#</td>
    <td>Total</td>
    <td>${total}</td>
    <td class=${status}>${roundedAverage} average</td>
    `;
    tableFooter.appendChild(row);
  }
  function parseGrade(gradeRaw) {
    const grade = {
      name: gradeRaw.name,
      value: gradeRaw.grade || "Not graded yet",
      status: "Fail"
    };

    if (grade.value >= 10) {
      grade.status = "Pass";
    } else if (grade.value === "Not graded yet") {
      grade.status = "Not graded yet";
    } else {
      grade.status = "Fail";
    }

    return grade;
  }
});
