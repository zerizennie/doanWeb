
$(document).ready(function(){
    $('.slider').slick({
        prevArrow:"<button type='button' class='slick-prev pull-left' style='color: #fa6e4f'><i class='fa-solid fa-circle-arrow-left' aria-hidden='true'></i></button>",
        nextArrow:"<button type='button' class='slick-next pull-right'><i class='fa-solid fa-circle-arrow-right' aria-hidden='true'></i></button>"

    });

    $('.three-slider').slick({
      dots: true,
      infinite: false,
      speed: 300,
      slidesToShow: 3,
      slidesToScroll: 3,
      responsive: [
        {
          breakpoint: 1024,
          settings: {
            slidesToShow: 2,
            slidesToScroll: 2,
            infinite: true,
            dots: true
          }
        },
        {
          breakpoint: 600,
          settings: {
            slidesToShow: 1,
            slidesToScroll: 1
          }
        },
        {
          breakpoint: 480,
          settings: {
            slidesToShow: 1,
            slidesToScroll: 1
          }
        }
        
      ],
        prevArrow:"<button type='button' class='slick-prev pull-left'><i class='fa-solid fa-circle-arrow-left' aria-hidden='true'></i></button>",
        nextArrow:"<button type='button' class='slick-next pull-right'><i class='fa-solid fa-circle-arrow-right' aria-hidden='true'></i></button>"
    });

    let mybutton = document.getElementById("btn-back-to-top");

// When the user scrolls down 20px from the top of the document, show the button
    window.onscroll = function () {
      scrollFunction();
    };

  function scrollFunction() {
    if (
      document.body.scrollTop > 20 ||
      document.documentElement.scrollTop > 20
    ) {
      mybutton.style.display = "block";
    } else {
      mybutton.style.display = "none";
    }
  }
// When the user clicks on the button, scroll to the top of the document
    mybutton.addEventListener("click", backToTop);

    function backToTop() {
      document.body.scrollTop = 0;
      document.documentElement.scrollTop = 0;
    }

    let userTexts = document.getElementsByClassName("user-text");
    let userPics = document.getElementsByClassName("user-pic");

  function showReview(event) {
    // Xóa lớp active-pic và active-text của tất cả các phần tử
    for (let userPic of userPics) {
      userPic.classList.remove("active-pic");
    }
    for (let userText of userTexts) {
      userText.classList.remove("active-text");
    }
  
    // Lấy chỉ số của phần tử user-pic đang được click
    let clickedPic = event.target;
    let i = Array.from(userPics).indexOf(clickedPic);
  
    // Thêm lớp active-pic và active-text cho phần tử tương ứng
    userPics[i].classList.add("active-pic");
    userTexts[i].classList.add("active-text");
  }
  
  // Gắn sự kiện click cho từng phần tử user-pic
  for (let userPic of userPics) {
    userPic.addEventListener('click', showReview);
  }
  //thanh menu cho reponsive
  var showhideButton = document.querySelector('.showhide');
  var dropdownMenu = document.querySelector('#dropdown-menu');
  
  showhideButton.addEventListener('click', function() {
  dropdownMenu.classList.toggle('show');
  });
  
  //hiện submenu
  $(document).ready(function() {
  $(".menu li.dropdown").hover(
  function() {
  $(this).find("ul.submenu").stop().slideDown();
  },
  function() {
  $(this).find("ul.submenu").stop().slideUp();
  }
  );
  });

})


  //Kiểm tra cho các trường khi điền form
document.getElementById('do_submit').addEventListener('click', function(event) {
  var nameInput = document.getElementById('name');
  var phoneInput = document.getElementById('phone');
  var emailInput = document.getElementById('email');
  var subjectInput = document.getElementById('subject');
  var contentInput = document.getElementById('content');
  
  if (nameInput.value.trim() === '' && emailInput.value.trim() === '' && phoneInput.value.trim() === '' && subjectInput.value.trim() === '' && contentInput.value.trim() === '') {
      event.preventDefault();
      alert('Vui lòng điền đầy đủ thông tin liên lạc.');
    }
  });


  window.addEventListener('DOMContentLoaded', function() {
    adjustReviewHeight();
  });

  window.addEventListener('resize', function() {
    adjustReviewHeight();
  });

  function adjustReviewHeight() {
    var reviewText = document.querySelector('.review-text');
    var activeText = document.querySelector('.user-text.active-text');

    if (activeText) {
      reviewText.style.height = activeText.offsetHeight + 'px';
    }
  }


 
