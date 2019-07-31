<template>
  <div>
    <mdb-row>
      <mdb-col col="1">
        <skyscrapper-banner></skyscrapper-banner>
      </mdb-col>
      <mdb-col class="text-center">
        <mdb-row>
          <mdb-col> <standard-banner></standard-banner><br /> </mdb-col>
          <mdb-col> <standard-banner></standard-banner><br /> </mdb-col>
        </mdb-row>

        <h2 class="text-secondary">
          Banner Credits: {{ this.$store.state.userData[0].BannerCredit }}
        </h2>
        <h2 class="text-secondary">
          Banner Day Credits:
          {{ this.$store.state.userData[0].BannerDayCredit }}
        </h2>
        <p class="text-primary">
          Note: You can only add banners of size 125*125 in this panel, anything
          other than this size will not work in this page.
        </p>
        <br />

        <mdb-btn
          v-on:click="advertisementSwitch = 1"
          color="secondary"
          size="sm"
          >Add new advertisement</mdb-btn
        >
        <mdb-btn
          v-on:click="advertisementSwitch = 2"
          color="secondary"
          size="sm"
          >Manage advertisement</mdb-btn
        >
        <mdb-btn
          v-on:click="advertisementSwitch = 3"
          color="secondary"
          size="sm"
          >Current advertisement</mdb-btn
        >
        <br />
        <br />

        <mdb-row v-if="advertisementSwitch === 1">
          <mdb-col>
            <mdb-card
              border="secondary"
              class="mb-3 mx-auto"
              style="max-width: 18rem;"
            >
              <mdb-card-header class="text-secondary"
                >Square Banner Impressions</mdb-card-header
              >
              <mdb-card-body class="text-secondary">
                <mdb-card-title tag="h5">Impressions Based</mdb-card-title>
                <mdb-card-text>
                  <p class="text-secondary">1 Credit = 1 Hit</p>

                  <mdb-input
                    basic
                    class="mb-3"
                    v-validate="'required'"
                    name="imageLink"
                    v-model.trim="imageLink"
                    type="text"
                  >
                    <span class="input-group-text" slot="prepend"
                      >Image Link</span
                    >
                  </mdb-input>

                  <mdb-input
                    basic
                    class="mb-3"
                    v-validate="'required'"
                    name="targetLink"
                    v-model.trim="targetLink"
                    type="text"
                  >
                    <span class="input-group-text" slot="prepend"
                      >Target Link</span
                    >
                  </mdb-input>

                  <mdb-input
                    basic
                    class="mb-3"
                    v-validate="
                      `required|min_value:0|max_value:${
                        this.$store.state.userData[0].BannerCredit
                      }`
                    "
                    name="credit"
                    v-model.trim="credit"
                    type="number"
                  >
                    <span class="input-group-text" slot="prepend">Credit</span>
                  </mdb-input>

                  <span class="text-danger">
                    {{ errors.first("imageLink") }} <br />
                    {{ errors.first("targetLink") }} <br />
                    {{ errors.first("credit") }}
                  </span>

                  <mdb-btn
                    v-on:click="bannerOrder()"
                    color="secondary"
                    size="sm"
                    >Add</mdb-btn
                  >
                </mdb-card-text>
              </mdb-card-body>
            </mdb-card>
          </mdb-col>
          <mdb-col>
            <mdb-card
              border="secondary"
              class="mb-3 mx-auto"
              style="max-width: 18rem;"
            >
              <mdb-card-header class="text-secondary"
                >Square Banner Day Impressions</mdb-card-header
              >
              <mdb-card-body class="text-secondary">
                <mdb-card-title tag="h5">Time Based</mdb-card-title>
                <mdb-card-text>
                  <p class="text-secondary">1 Credit = 1 Day</p>

                  <mdb-input
                    basic
                    class="mb-3"
                    v-validate="'required'"
                    name="imageLink"
                    v-model.trim="imageLink"
                    type="text"
                  >
                    <span class="input-group-text" slot="prepend"
                      >Image Link</span
                    >
                  </mdb-input>

                  <mdb-input
                    basic
                    class="mb-3"
                    v-validate="'required'"
                    name="targetLink"
                    v-model.trim="targetLink"
                    type="text"
                  >
                    <span class="input-group-text" slot="prepend"
                      >Target Link</span
                    >
                  </mdb-input>

                  <mdb-input
                    basic
                    class="mb-3"
                    v-validate="
                      `required|min_value:0|max_value:${
                        this.$store.state.userData[0].BannerDayCredit
                      }`
                    "
                    name="dayCredit"
                    v-model.trim="dayCredit"
                    type="number"
                  >
                    <span class="input-group-text" slot="prepend">Credit</span>
                  </mdb-input>

                  <span class="text-danger">
                    {{ errors.first("imageLink") }} <br />
                    {{ errors.first("targetLink") }} <br />
                    {{ errors.first("dayCredit") }}
                  </span>

                  <mdb-btn
                    v-on:click="bannerDayOrder()"
                    color="secondary"
                    size="sm"
                    >Add</mdb-btn
                  >
                </mdb-card-text>
              </mdb-card-body>
            </mdb-card>
          </mdb-col>
        </mdb-row>

        <mdb-row v-if="advertisementSwitch === 2">
          <mdb-col>
            <mdb-card
              border="secondary"
              class="mb-3 mx-auto"
              style="max-width: 18rem;"
            >
              <mdb-card-header class="text-secondary"
                >Manage Square Banner Advertisement</mdb-card-header
              >
              <mdb-card-body class="text-secondary">
                <mdb-card-title tag="h5"
                  >Note: You cannot add credits in time based
                  advertisement</mdb-card-title
                >

                <mdb-btn
                  v-on:click="manageSquareBannerSwitch = 1"
                  color="secondary"
                  size="sm"
                  >Impressions Based</mdb-btn
                >
                <mdb-btn
                  v-on:click="manageSquareBannerSwitch = 2"
                  color="secondary"
                  size="sm"
                  >Time Based</mdb-btn
                >
                <br />
                <br />

                <mdb-card-text v-if="manageSquareBannerSwitch === 1">
                  <mdb-input
                    basic
                    class="mb-3"
                    v-validate="'required'"
                    name="reference"
                    v-model.trim="reference"
                    type="text"
                  >
                    <span class="input-group-text" slot="prepend"
                      >Reference</span
                    >
                  </mdb-input>

                  <mdb-input
                    basic
                    class="mb-3"
                    v-validate="'required'"
                    name="imageLink"
                    v-model.trim="imageLink"
                    type="text"
                  >
                    <span class="input-group-text" slot="prepend"
                      >Image Link</span
                    >
                  </mdb-input>

                  <mdb-input
                    basic
                    class="mb-3"
                    v-validate="'required'"
                    name="targetLink"
                    v-model.trim="targetLink"
                    type="text"
                  >
                    <span class="input-group-text" slot="prepend"
                      >Target Link</span
                    >
                  </mdb-input>

                  <mdb-input
                    basic
                    class="mb-3"
                    v-validate="
                      `required|min_value:0|max_value:${
                        this.$store.state.userData[0].BannerCredit
                      }`
                    "
                    name="credit"
                    v-model.trim="credit"
                    type="number"
                  >
                    <span class="input-group-text" slot="prepend"
                      >Add Credit</span
                    >
                  </mdb-input>

                  <span class="text-danger">
                    {{ errors.first("reference") }} <br />
                    {{ errors.first("imageLink") }} <br />
                    {{ errors.first("targetLink") }} <br />
                    {{ errors.first("credit") }} <br />
                    <span v-if="doesntExist"
                      >You do not have any such referenced square banner
                      order.</span
                    ><br />
                  </span>

                  <mdb-btn
                    v-on:click="updateBannerOrder()"
                    color="secondary"
                    size="sm"
                    >Update</mdb-btn
                  >
                </mdb-card-text>

                <mdb-card-text v-if="manageSquareBannerSwitch === 2">
                  <mdb-input
                    basic
                    class="mb-3"
                    v-validate="'required'"
                    name="reference"
                    v-model.trim="reference"
                    type="text"
                  >
                    <span class="input-group-text" slot="prepend"
                      >Reference</span
                    >
                  </mdb-input>

                  <mdb-input
                    basic
                    class="mb-3"
                    v-validate="'required'"
                    name="imageLink"
                    v-model.trim="imageLink"
                    type="text"
                  >
                    <span class="input-group-text" slot="prepend"
                      >Image Link</span
                    >
                  </mdb-input>

                  <mdb-input
                    basic
                    class="mb-3"
                    v-validate="'required'"
                    name="targetLink"
                    v-model.trim="targetLink"
                    type="text"
                  >
                    <span class="input-group-text" slot="prepend"
                      >Target Link</span
                    >
                  </mdb-input>

                  <span class="text-danger">
                    {{ errors.first("reference") }} <br />
                    {{ errors.first("imageLink") }} <br />
                    {{ errors.first("targetLink") }} <br />
                    <span v-if="doesntExist"
                      >You do not have any such referenced square banner
                      order.</span
                    ><br />
                  </span>

                  <mdb-btn
                    v-on:click="updateBannerDayOrder()"
                    color="secondary"
                    size="sm"
                    >Update</mdb-btn
                  >
                </mdb-card-text>
              </mdb-card-body>
            </mdb-card>
          </mdb-col>
        </mdb-row>

        <mdb-row v-if="advertisementSwitch === 3">
          <mdb-col>
            <mdb-datatable
              v-bind:data="this.$store.state.reactiveSquareBannerOrderData"
              v-bind:searching="true"
              class="w-75"
              v-bind:pagination="true"
              striped
              bordered
              hover
              small
              autoWidth
              responsive
            />
          </mdb-col>
        </mdb-row>
      </mdb-col>
      <mdb-col col="1">
        <skyscrapper-banner></skyscrapper-banner>
      </mdb-col>
      <mdb-col col="1"> </mdb-col>
    </mdb-row>

    <mdb-row>
      <mdb-col>
        <standard-banner></standard-banner><br />
        <standard-banner></standard-banner><br />
        <standard-banner></standard-banner><br />
        <standard-banner></standard-banner><br />
        <standard-banner></standard-banner><br />
        <standard-banner></standard-banner><br />
      </mdb-col>
      <mdb-col>
        <standard-banner></standard-banner><br />
        <standard-banner></standard-banner><br />
        <standard-banner></standard-banner><br />
        <standard-banner></standard-banner><br />
        <standard-banner></standard-banner><br />
        <standard-banner></standard-banner><br />
      </mdb-col>
    </mdb-row>

    <button
      type="button"
      class="bottomLeftButton"
      id="bottomLeftButton"
      onClick="document.getElementById('bottomLeftBanner').remove();this.remove();"
    >
      x
    </button>
    <div class="bottomLeftBanner" id="bottomLeftBanner">
      <square-banner></square-banner>
    </div>

    <button
      type="button"
      class="bottomRightButton"
      id="bottomRightButton"
      onClick="document.getElementById('bottomRightBanner').remove();this.remove();"
    >
      x
    </button>
    <div class="bottomRightBanner" id="bottomRightBanner">
      <square-banner></square-banner>
    </div>
  </div>
</template>

<script>
import {
  mdbRow,
  mdbCol,
  mdbBtn,
  mdbCard,
  mdbCardHeader,
  mdbCardBody,
  mdbCardTitle,
  mdbCardText,
  mdbInput,
  mdbDatatable
} from "mdbvue";

export default {
  components: {
    mdbRow,
    mdbCol,
    mdbBtn,
    mdbCard,
    mdbCardBody,
    mdbCardTitle,
    mdbCardText,
    mdbCardHeader,
    mdbInput,
    mdbDatatable
  },
  data() {
    return {
      reference: null,
      imageLink: null,
      targetLink: null,
      link: "Empty link",
      credit: null,
      dayCredit: null,
      advertisementSwitch: 1,
      manageSquareBannerSwitch: 1,
      doesntExist: false
    };
  },
  methods: {
    test: function() {},
    bannerOrder: function() {
      this.$validator
        .validateAll({
          imageLink: this.imageLink,
          targetLink: this.targetLink,
          credit: this.credit
        })
        .then(result => {
          if (result === true) {
            this.$socket.emit(
              "placingAdvertisingOrder",
              JSON.stringify({
                sessionId: this.$cookie.get("sessionId"),
                name: "SquareBanner",
                imageLink: this.imageLink,
                targetLink: this.targetLink,
                isTimeBasedInput: "0",
                creditInput: this.credit
              })
            );
          }
        });
    },
    bannerDayOrder: function() {
      this.$validator
        .validateAll({
          imageLink: this.imageLink,
          targetLink: this.targetLink,
          dayCredit: this.dayCredit
        })
        .then(result => {
          if (result === true) {
            this.$socket.emit(
              "placingAdvertisingOrder",
              JSON.stringify({
                sessionId: this.$cookie.get("sessionId"),
                name: "SquareBanner",
                imageLink: this.imageLink,
                targetLink: this.targetLink,
                isTimeBasedInput: "1",
                creditInput: this.dayCredit
              })
            );
          }
        });
    },
    updateBannerOrder: function() {
      this.doesntExist = false;
      var hasAny = false;

      this.$validator
        .validateAll({
          reference: this.reference,
          imageLink: this.imageLink,
          targetLink: this.targetLink,
          credit: this.credit
        })
        .then(result => {
          if (result === true) {
            for (
              var i = 0;
              i < this.$store.state.reactiveSquareBannerOrderData.rows.length;
              i++
            ) {
              if (
                this.reference ===
                  this.$store.state.reactiveSquareBannerOrderData.rows[i]
                    .reference &&
                this.$store.state.reactiveSquareBannerOrderData.rows[i]
                  .isTimeBased === "false"
              ) {
                hasAny = true;
                break;
              }
            }

            if (hasAny === true) {
              this.$socket.emit(
                "updatingAdvertisingOrder",
                JSON.stringify({
                  sessionId: this.$cookie.get("sessionId"),
                  reference: this.reference,
                  name: "SquareBanner",
                  imageLink: this.imageLink,
                  targetLink: this.targetLink,
                  isTimeBasedInput: "0",
                  creditInput: this.credit
                })
              );
            } else if (hasAny === false) {
              this.doesntExist = true;
            }
          }
        });
    },

    updateBannerDayOrder: function() {
      this.doesntExist = false;
      var hasAny = false;

      this.$validator
        .validateAll({
          reference: this.reference,
          imageLink: this.imageLink,
          targetLink: this.targetLink
        })
        .then(result => {
          if (result === true) {
            for (
              var i = 0;
              i < this.$store.state.reactiveSquareBannerOrderData.rows.length;
              i++
            ) {
              if (
                this.reference ===
                  this.$store.state.reactiveSquareBannerOrderData.rows[i]
                    .reference &&
                this.$store.state.reactiveSquareBannerOrderData.rows[i]
                  .isTimeBased === "true"
              ) {
                hasAny = true;
                break;
              }
            }

            if (hasAny === true) {
              this.$socket.emit(
                "updatingAdvertisingOrder",
                JSON.stringify({
                  sessionId: this.$cookie.get("sessionId"),
                  reference: this.reference,
                  name: "SquareBanner",
                  imageLink: this.imageLink,
                  targetLink: this.targetLink,
                  isTimeBasedInput: "1",
                  creditInput: 0
                })
              );
            } else if (hasAny === false) {
              this.doesntExist = true;
            }
          }
        });
    }
  },
  mounted() {
    this.$store.commit("isLoading", true);

    this.$store.commit("isLoading", false);
  }
};
</script>
