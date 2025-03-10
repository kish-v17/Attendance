const tabs = document.querySelectorAll(".tab-item");
const tabContents = document.querySelectorAll(".tab-content");

tabs.forEach((tab) => {
    tab.addEventListener("click", () => {
        tabs.forEach((t) => t.classList.remove("active"));
        tabContents.forEach((tc) => tc.classList.remove("active"));

        tab.classList.add("active");
        const tabId = tab.getAttribute("data-tab") + "-content";
        document.getElementById(tabId).classList.add("active");
    });
});

document.addEventListener("DOMContentLoaded", function () {
    const burgerMenu = document.querySelector(".burger-menu");
    const sidebar = document.querySelector(".sidebar");

    if (burgerMenu && sidebar) {
        burgerMenu.addEventListener("click", function (e) {
            e.stopPropagation();
            sidebar.classList.toggle("active");

            // Hide burger button when sidebar is active
            if (sidebar.classList.contains("active")) {
                burgerMenu.style.display = "none";
            }
        });

        // Close sidebar when clicking outside and show burger button again
        document.addEventListener("click", function (e) {
            if (!sidebar.contains(e.target) && !burgerMenu.contains(e.target)) {
                sidebar.classList.remove("active");
                burgerMenu.style.display = "block"; // Show burger button again
            }
        });
    }
});