const coursesSelect = document.getElementById('courses');
const studentsSelect = document.getElementById('students');

coursesSelect.addEventListener('change', async function () {
    const courseId = coursesSelect.value;
    const response = await fetch(`GeAvailableStudents?courseId=${courseId}`);

    if (response.ok) {
        const students = await response.json();
        studentsSelect.innerHTML = '';
        students.forEach(student => {
            const option = document.createElement('option');
            option.value = student.id;
            option.text = student.name;
            studentsSelect.add(option);
        });

        if (students.length === 0) {
            const option = document.createElement('option');
            option.text = 'No students available, please select another course';
            option.disabled = true;
            studentsSelect.add(option);
        }

        const option = document.createElement('option');
        option.text = 'Select Student to enroll';
        option.disabled = true;
        studentsSelect.insertBefore(option, studentsSelect.firstChild);
    } else {
      studentsSelect.innerHTML = '';
      const option = document.createElement('option');
      option.text = 'Select Student to enroll';
      studentsSelect.add(option);
      alert('Failed to load students, please try again');
    }
});
