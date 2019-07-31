<template>
  <div id="app">
    <main>
      <router-view></router-view>
    </main>
  </div>
</template>
<script>
export default {
  components: {},
  data() {
    return {};
  },
  sockets: {},
  methods: {
    test: function() {}
  },
  mounted() {}
};
</script>

<style>
.bottomLeftButton {
  bottom: 120px;
  left: 120px;
  position: fixed;
  border-radius: 50%;
  display: block;
  background-color: black;
  color: white;
}

.bottomLeftBanner {
  bottom: 0;
  left: 0;
  position: fixed;
}

.bottomRightButton {
  bottom: 120px;
  right: 120px;
  position: fixed;
  border-radius: 50%;
  display: block;
  background-color: black;
  color: white;
}

.bottomRightBanner {
  bottom: 0;
  right: 0;
  position: fixed;
}

.bottomSecondRightButton {
  bottom: 120px;
  right: 240px;
  position: fixed;
  border-radius: 50%;
  display: block;
  background-color: black;
  color: white;
}

.bottomSecondRightBanner {
  bottom: 0;
  right: 250px;
  position: fixed;
}

.bottomSecondLeftButton {
  bottom: 120px;
  left: 240px;
  position: fixed;
  border-radius: 50%;
  display: block;
  background-color: black;
  color: white;
}

.bottomSecondLeftBanner {
  bottom: 0;
  left: 250px;
  position: fixed;
}

.bottomStandardButton {
  bottom: 160px;
  left: 450px;
  position: fixed;
  border-radius: 50%;
  display: block;
  background-color: black;
  color: white;
}

.bottomStandardBanner {
  bottom: 0;
  left: 450px;
  position: fixed;
}

.bottomSecondStandardBanner {
  bottom: 85px;
  left: 450px;
  position: fixed;
}
</style>
