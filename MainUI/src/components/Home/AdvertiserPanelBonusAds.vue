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
          Bonus Ad Credits: {{ this.$store.state.userData[0].BonusAdCredit }}
        </h2>
        <h2 class="text-secondary">
          Bonus Ad Day Credits:
          {{ this.$store.state.userData[0].BonusAdDayCredit }}
        </h2>
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
                >Bonus Ads Traffic</mdb-card-header
              >
              <mdb-card-body class="text-secondary">
                <mdb-card-title tag="h5">Hits Based</mdb-card-title>
                <mdb-card-text>
                  <p class="text-secondary">1 Credit = 1 Hit</p>

                  <mdb-input basic class="mb-3" name="title" type="text">
                    <span
                      class="input-group-text"
                      v-validate="'max:20'"
                      slot="prepend"
                      >Title (Optional)</span
                    >
                  </mdb-input>

                  <mdb-input basic class="mb-3" name="description" type="text">
                    <span
                      class="input-group-text"
                      v-validate="'max:50'"
                      slot="prepend"
                      >Description (Optional)</span
                    >
                  </mdb-input>

                  <mdb-input
                    basic
                    class="mb-3"
                    v-validate="'required'"
                    name="link"
                    v-model.trim="link"
                    type="text"
                  >
                    <span class="input-group-text" slot="prepend">Link</span>
                  </mdb-input>

                  <mdb-input
                    basic
                    class="mb-3"
                    v-validate="
                      `required|min_value:0|max_value:${
                        this.$store.state.userData[0].BonusAdCredit
                      }`
                    "
                    name="credit"
                    v-model.trim="credit"
                    type="number"
                  >
                    <span class="input-group-text" slot="prepend">Credit</span>
                  </mdb-input>

                  <span class="text-danger">
                    {{ errors.first("link") }} <br />
                    {{ errors.first("title") }} <br />
                    {{ errors.first("description") }} <br />
                    {{ errors.first("credit") }}
                  </span>

                  <mdb-btn
                    v-on:click="bonusAdOrder()"
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
                >Bonus Ad Day Traffic</mdb-card-header
              >
              <mdb-card-body class="text-secondary">
                <mdb-card-title tag="h5">Time Based</mdb-card-title>
                <mdb-card-text>
                  <p class="text-secondary">1 Credit = 1 Day</p>

                  <mdb-input basic class="mb-3" name="title" type="text">
                    <span
                      class="input-group-text"
                      v-validate="'max:20'"
                      slot="prepend"
                      >Title (Optional)</span
                    >
                  </mdb-input>

                  <mdb-input basic class="mb-3" name="description" type="text">
                    <span
                      class="input-group-text"
                      v-validate="'max:50'"
                      slot="prepend"
                      >Description (Optional)</span
                    >
                  </mdb-input>

                  <mdb-input
                    basic
                    class="mb-3"
                    v-validate="'required'"
                    name="link"
                    v-model.trim="link"
                    type="text"
                  >
                    <span class="input-group-text" slot="prepend">Link</span>
                  </mdb-input>

                  <mdb-input
                    basic
                    class="mb-3"
                    v-validate="
                      `required|min_value:0|max_value:${
                        this.$store.state.userData[0].BonusAdDayCredit
                      }`
                    "
                    name="dayCredit"
                    v-model.trim="dayCredit"
                    type="number"
                  >
                    <span class="input-group-text" slot="prepend">Credit</span>
                  </mdb-input>

                  <span class="text-danger">
                    {{ errors.first("link") }} <br />
                    {{ errors.first("dayCredit") }}
                  </span>

                  <mdb-btn
                    v-on:click="bonusAdDayOrder()"
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
                >Manage Bonus Ad Advertisement</mdb-card-header
              >
              <mdb-card-body class="text-secondary">
                <mdb-card-title tag="h5"
                  >Note: You cannot add credits in time based
                  advertisement</mdb-card-title
                >

                <mdb-btn
                  v-on:click="manageBonusAdSwitch = 1"
                  color="secondary"
                  size="sm"
                  >Hits Based</mdb-btn
                >
                <mdb-btn
                  v-on:click="manageBonusAdSwitch = 2"
                  color="secondary"
                  size="sm"
                  >Time Based</mdb-btn
                >
                <br />
                <br />

                <mdb-card-text v-if="manageBonusAdSwitch === 1">
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

                  <mdb-input basic class="mb-3" name="title" type="text">
                    <span
                      class="input-group-text"
                      v-validate="'max:20'"
                      slot="prepend"
                      >Title (Optional)</span
                    >
                  </mdb-input>

                  <mdb-input basic class="mb-3" name="description" type="text">
                    <span
                      class="input-group-text"
                      v-validate="'max:50'"
                      slot="prepend"
                      >Description (Optional)</span
                    >
                  </mdb-input>

                  <mdb-input
                    basic
                    class="mb-3"
                    v-validate="'required'"
                    name="modifyLink"
                    v-model.trim="modifyLink"
                    type="text"
                  >
                    <span class="input-group-text" slot="prepend"
                      >Modify Link</span
                    >
                  </mdb-input>

                  <mdb-input
                    basic
                    class="mb-3"
                    v-validate="
                      `required|min_value:0|max_value:${
                        this.$store.state.userData[0].BonusAdCredit
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
                    {{ errors.first("title") }} <br />
                    {{ errors.first("description") }} <br />
                    {{ errors.first("modifyLink") }} <br />
                    {{ errors.first("credit") }} <br />
                    <span v-if="doesntExist"
                      >You do not have any such referenced ptp order.</span
                    ><br />
                  </span>

                  <mdb-btn
                    v-on:click="updateBonusAdOrder()"
                    color="secondary"
                    size="sm"
                    >Update</mdb-btn
                  >
                </mdb-card-text>

                <mdb-card-text v-if="manageBonusAdSwitch === 2">
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

                  <mdb-input basic class="mb-3" name="title" type="text">
                    <span
                      class="input-group-text"
                      v-validate="'max:20'"
                      slot="prepend"
                      >Title (Optional)</span
                    >
                  </mdb-input>

                  <mdb-input basic class="mb-3" name="description" type="text">
                    <span
                      class="input-group-text"
                      v-validate="'max:50'"
                      slot="prepend"
                      >Description (Optional)</span
                    >
                  </mdb-input>

                  <mdb-input
                    basic
                    class="mb-3"
                    v-validate="'required'"
                    name="modifyLink"
                    v-model.trim="modifyLink"
                    type="text"
                  >
                    <span class="input-group-text" slot="prepend"
                      >Modify Link</span
                    >
                  </mdb-input>

                  <span class="text-danger">
                    {{ errors.first("reference") }} <br />
                    {{ errors.first("title") }} <br />
                    {{ errors.first("description") }} <br />
                    {{ errors.first("modifyLink") }} <br />
                    <span v-if="doesntExist"
                      >You do not have any such referenced bonus ad order.</span
                    ><br />
                  </span>

                  <mdb-btn
                    v-on:click="updateBonusAdDayOrder()"
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
              v-bind:data="this.$store.state.reactiveBonusAdOrderData"
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
      modifyLink: null,
      title: null,
      description: null,
      link: null,
      credit: null,
      dayCredit: null,
      advertisementSwitch: 1,
      manageBonusAdSwitch: 1,
      doesntExist: false
    };
  },
  methods: {
    test: function() {},
    bonusAdOrder: function() {
      this.$validator
        .validateAll({
          link: this.link,
          title: this.title,
          description: this.description,
          credit: this.credit
        })
        .then(result => {
          if (result === true) {
            this.$socket.emit(
              "placingAdvertisingOrder",
              JSON.stringify({
                sessionId: this.$cookie.get("sessionId"),
                name: "BonusAd",
                title: this.title,
                description: this.description,
                link: this.link,
                isTimeBasedInput: "0",
                creditInput: this.credit
              })
            );
          }
        });
    },
    bonusAdDayOrder: function() {
      this.$validator
        .validateAll({
          title: this.title,
          description: this.description,
          link: this.link,
          dayCredit: this.dayCredit
        })
        .then(result => {
          if (result === true) {
            this.$socket.emit(
              "placingAdvertisingOrder",
              JSON.stringify({
                sessionId: this.$cookie.get("sessionId"),
                name: "BonusAd",
                title: this.title,
                description: this.description,
                link: this.link,
                isTimeBasedInput: "1",
                creditInput: this.dayCredit
              })
            );
          }
        });
    },
    updateBonusAdOrder: function() {
      this.doesntExist = false;
      var hasAny = false;

      this.$validator
        .validateAll({
          reference: this.reference,
          title: this.title,
          description: this.description,
          modifyLink: this.modifyLink,
          credit: this.credit
        })
        .then(result => {
          if (result === true) {
            for (
              var i = 0;
              i < this.$store.state.reactiveBonusAdOrderData.rows.length;
              i++
            ) {
              if (
                this.reference ===
                  this.$store.state.reactiveBonusAdOrderData.rows[i]
                    .reference &&
                this.$store.state.reactiveBonusAdOrderData.rows[i]
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
                  name: "BonusAd",
                  title: this.title,
                  description: this.description,
                  link: this.modifyLink,
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

    updateBonusAdDayOrder: function() {
      this.doesntExist = false;
      var hasAny = false;

      this.$validator
        .validateAll({
          reference: this.reference,
          title: this.title,
          description: this.description,
          modifyLink: this.modifyLink
        })
        .then(result => {
          if (result === true) {
            for (
              var i = 0;
              i < this.$store.state.reactiveBonusAdOrderData.rows.length;
              i++
            ) {
              if (
                this.reference ===
                  this.$store.state.reactiveBonusAdOrderData.rows[i]
                    .reference &&
                this.$store.state.reactiveBonusAdOrderData.rows[i]
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
                  name: "BonusAd",
                  title: this.title,
                  description: this.description,
                  link: this.modifyLink,
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
