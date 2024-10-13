console.log("showGrade.js loaded");


//global variables
const caption = document.getElementById("caption");
const tableBody = document.getElementById("tableBody");
const tableFooter = document.getElementById("tableFooter");

const ShowGradesBody = (data) =>  {
  tableBody.innerHTML = "";
  data.grades.forEach((grade) => {
    const tr = document.createElement('tr');

    const dt1 = document.createElement('td');
    dt1.textContent = grade.subjectName;
    tr.appendChild(dt1);

    const dt2 = document.createElement('td');
    const span = document.createElement('span');
    if (grade.expectedHours == 0) {
      span.className = "badge rounded-pill bg-label-warning";
      span.textContent = "No expected hours";
    } else {
      span.className = "badge rounded-pill bg-label-primary";
      span.textContent = grade.expectedHours;
    }
    dt2.appendChild(span);
    tr.appendChild(dt2);

    const dt3 = document.createElement('td');
    const span2 = document.createElement('span');
    if (grade.absents == 0) {
      span2.className = "badge rounded-pill bg-label-success";
      span2.textContent = "No absents";
    } else {
      span2.className = "badge rounded-pill bg-label-danger";
      span2.textContent = grade.absents;
    }
    dt3.appendChild(span2);
    tr.appendChild(dt3);

    const dt4 = document.createElement('td');
    const span3 = document.createElement('span');
    if (grade.grade == 0) {
      span3.className = "badge rounded-pill bg-label-warning";
      span3.textContent = "Not graded yet";
    } else {
      if (grade.grade >= 10) {
        span3.className = "badge rounded-pill bg-label-success";
        span3.textContent = grade.grade;
      } else {
        span3.className = "badge rounded-pill bg-label-danger";
        span3.textContent = grade.grade;
      }
    }
    dt4.appendChild(span3);
    tr.appendChild(dt4);

    const dt5 = document.createElement('td');
    const span4 = document.createElement('span');
    if (grade.isExcluded) {
      span4.className = "badge rounded-pill bg-label-danger";
      span4.textContent = `${grade.attendancePercentage}%`;
    } else {
      span4.className = "badge rounded-pill bg-label-success";
      span4.textContent = `${grade.attendancePercentage}%`;
    }
    dt5.appendChild(span4);
    tr.appendChild(dt5);

    const dt6 = document.createElement('td');
    const span5 = document.createElement('span');
    if (grade.isExcluded) {
      span5.className = "badge rounded-pill bg-label-danger";
      span5.textContent = "Excluded for Absents";
    } else {
      if (grade.status == "Pass") {
        span5.className = "badge rounded-pill bg-label-success";
        span5.textContent = "Approved";
      } else if (grade.status == "Fail") {
        span5.className = "badge rounded-pill bg-label-danger";
        span5.textContent = "Not approved";
      } else {
        span5.className = "badge rounded-pill bg-label-warning";
        span5.textContent = "Pending";
      }
    }
    dt6.appendChild(span5);
    tr.appendChild(dt6);
    tableBody.appendChild(tr);

  });
};
const ShowGradesFooter = (data) =>  {
  tableFooter.innerHTML = '';
  const tr = document.createElement('tr');

  const th1 = document.createElement('th');
  th1.className = "rounded-start-bottom";
  th1.textContent = "Total";
  tr.appendChild(th1);

  const th2 = document.createElement('th');
  const span1 = document.createElement('span');
  if (data.totalExpectedHours > 0) {
    span1.className = "badge rounded-pill bg-label-primary";
    span1.textContent = data.totalExpectedHours;
  } else {
    span1.className = "badge rounded-pill bg-label-warning";
    span1.textContent = "No expected hours";
  }
  th2.appendChild(span1);
  tr.appendChild(th2);

  const th3 = document.createElement('th');
  const span2 = document.createElement('span');
  if (data.totalAbsents > 0) {
    span2.className = "badge rounded-pill bg-label-danger";
    span2.textContent = data.totalAbsents;
  } else {
    span2.className = "badge rounded-pill bg-label-primary";
    span2.textContent = "No absents";
  }
  th3.appendChild(span2);
  tr.appendChild(th3);

  const th4 = document.createElement('th');
  const span3 = document.createElement('span');
  if (data.average > 0) {
    span3.className = "badge rounded-pill bg-label-primary";
    span3.textContent = `${data.average} Average`;
  } else {
    span3.className = "badge rounded-pill bg-label-warning";
    span3.textContent = "No grades yet";
  }
  th4.appendChild(span3);
  tr.appendChild(th4);

  const th5 = document.createElement('th');
  const span4 = document.createElement('span');
  if (data.totalAttendancePercentage >= 75) {
    span4.className = "badge rounded-pill bg-label-success";
    span4.textContent = `${data.totalAttendancePercentage}%`;
  } else {
    span4.className = "badge rounded-pill bg-label-danger";
    span4.textContent = `${data.totalAttendancePercentage}%`;
  }
  th5.appendChild(span4);
  tr.appendChild(th5);

  const th6 = document.createElement('th');
  const span5 = document.createElement('span');
  if (data.isTotalExcluded) {
    span5.className = "badge rounded-pill bg-label-danger";
    span5.textContent = "Excluded for Absents";
  } else {
    if (data.totalStatus == "Pass") {
      span5.className = "badge rounded-pill bg-label-success";
      span5.textContent = "Approved";
    } else if (data.totalStatus == "Fail") {
      span5.className = "badge rounded-pill bg-label-danger";
      span5.textContent = "Not approved";
    } else {
      span5.className = "badge rounded-pill bg-label-warning";
      span5.textContent = "Pending";
    }
  }
  th6.appendChild(span5);
  tr.appendChild(th6);

  tableFooter.appendChild(tr);
};
document.addEventListener('DOMContentLoaded', function () {

    // Get the button
    let studentBtn = document.querySelectorAll('.showGradeButton');

    // Add event listener to the button
    studentBtn.forEach(async function (btn) {
      btn.addEventListener('click', async function () {
              tableBody.innerHTML = '';
              tableFooter.innerHTML = '';
              const courseId = btn.dataset.course;
              const studentId = btn.dataset.student;
              const studentName = btn.dataset.author;

              caption.innerHTML = `Grades for ${studentName}`;
              const row = document.createElement('tr');
              row.innerHTML = `
              <td colspan="4">Loading...</td>
              `;
              tableBody.appendChild(row);
              await GetAllGradesOfStudent(courseId, studentId);
      });
    });

  async function GetAllGradesOfStudent(courseId, studentId) {

    const response = await fetch(`GetStudentGrades/?courseId=${courseId}&studentId=${studentId}`);

    if (response.ok) {
      const data = await response.json();
      debugger;
      if (data.grades.length > 0) {
        ShowGradesBody(data);
        ShowGradesFooter(data);
      } else {
        tableBody.innerHTML = '';
        const row = document.createElement('tr');
        row.innerHTML = `
        <td colspan="6" class="text-center">No grades found for this student</td>
        `;
        tableBody.appendChild(row);
      }
    } else {
      console.error('Failed to fetch data');
      alert('Failed to fetch student grades, please try again later');
    }
  }
});
