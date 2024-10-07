document.addEventListener('DOMContentLoaded', function () {

    const studentsBtn = document.querySelectorAll('.ListStudent');
    const displayName = document.getElementById("StudentNameDisplay");

    studentsBtn.forEach(function (btn) {
        btn.addEventListener('click', function () {
            const studentName = btn.dataset.author;
            const studentId = btn.dataset.student;
            const courseId = btn.dataset.course;

            displayName.innerHTML = `In what subject ${studentName} was absent?`;

            const btnMark = document.getElementById("btnMark");

            btnMark.onclick = async function (e) {
                e.preventDefault();
                await markAbsentAsync(studentId, courseId, studentName);
            }
        });
    });
});

const markAbsentAsync = async (studentId, courseId, studentName) => {
    var Obj = document.getElementById("Subjects").ej2_instances[0];

    // Coudn't find a way to get the selected item from the dropdown
    // So I'm getting the selected item by the dropdown's instance
    const selectedSubject = Obj.itemData;
    if (!selectedSubject) {
        alert("Please select a subject!");
        return;
    }

    const data = {
        studentId,
        courseId,
        subjectId: selectedSubject.Id  // Id of the selected subject by syncfusion dropdown
    };

    const response = await fetch("MarkAbsent", {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    });

    if (response.ok) {
        // alert("Absent marked successfully!");
        let toastObj = document.getElementById('element').ej2_instances[0];
        toastObj.target = document.body;
        toastObj.title = 'Success';
        toastObj.content = `Absent marked successfully for ${studentName} on subject ${selectedSubject.Name}!`;
        const btnClose = document.getElementById("btnCloseModal");
        btnClose.click();
        toastObj.show();

        setTimeout(() => {
          toastObj.hide();
        }, 3000)
    } else {
        alert("Something went wrong, please try again!");
    }
}
