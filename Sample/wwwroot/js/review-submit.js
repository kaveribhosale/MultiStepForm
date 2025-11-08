let autoSaveInterval;

document.addEventListener("DOMContentLoaded", () => {
    startAutoSave();

    document.getElementById("saveDraftBtn").addEventListener("click", async () => {
        await saveDraft();
        alert("Draft saved successfully.");
    });

    document.getElementById("submitBtn").addEventListener("click", async (e) => {
        e.preventDefault();
        await submitApplication();
    });
});

function startAutoSave() {
    autoSaveInterval = setInterval(saveDraft, 10000);
}

function stopAutoSave() {
    clearInterval(autoSaveInterval);
}

async function saveDraft() {
    const form = document.getElementById("applicationForm");
    const formData = new FormData(form);

    try {
        const response = await fetch("/Application/SaveDraft", {
            method: "POST",
            body: formData
        });

        const data = await response.json();
        if (data.id && form.querySelector("input[name='Id']").value == 0) {
            form.querySelector("input[name='Id']").value = data.id;
        }

        console.log("Auto-saved draft, Id:", data.id);
    } catch (err) {
        console.error("Auto-save failed:", err);
    }
}

async function submitApplication() {
    stopAutoSave();

    const form = document.getElementById("applicationForm");
    const formData = new FormData(form);

    try {
        const response = await fetch("/Application/SubmitApplication", {
            method: "POST",
            body: formData
        });

        if (response.redirected) {
            window.location.href = response.url; 
        } else if (response.ok) {
            const data = await response.json();
            window.location.href = `/Application/Summary/${data.id}`;
        } else {
            alert("Something went wrong while submitting.");
        }
    } catch (err) {
        console.error("Submit failed:", err);
        alert("Error submitting application.");
    }
}
