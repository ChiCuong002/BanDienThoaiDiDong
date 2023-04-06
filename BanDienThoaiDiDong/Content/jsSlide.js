window.addEventListener("load",function(){
   const slider = this.document.querySelector('.slider');
   const sliderMain = this.document.querySelector('.slider-main');
   const sliderItems = this.document.querySelectorAll('.slider-item');

   const nextBtn = this.document.querySelector('.slider-next');
   const prevBtn = this.document.querySelector('.slider-prev');

   const sliderItemWidth = sliderItems[0].offsetWidth;
   const sliderLenght = sliderItems.length
   let postionX=0;
   let index =0;
   
   nextBtn.addEventListener('click',function(){
    handleChangeSlide(1);
   });
   prevBtn.addEventListener('click',function(){
    handleChangeSlide(-1);
   });
   this.setInterval(function(){
        handleChangeSlide(1);
   },2000);
   function handleChangeSlide(dir){
    
    if(dir===1){
        index++;
        
        if(index >= sliderLenght){
            index=0;
            postionX=0;
        }else{
            postionX=postionX-sliderItemWidth;
        }
        
        sliderMain.style=`transform: translateX(${postionX}px)`;
    }else{
        index--;
        if(index < 0){
            index=sliderLenght-1;
            postionX=-sliderItemWidth*(sliderLenght-1);
           
        }else{
            postionX=postionX+sliderItemWidth;
        }
        
        sliderMain.style=`transform: translateX(${postionX}px)`;

        
    }
   }
});